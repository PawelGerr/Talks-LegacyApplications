using System;
using System.IO;
using System.IO.Pipes;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Ipc.NamedPipes
{
	public class NamedPipesClient
	{
		private readonly IObservable<object> _received;

		public Guid OwnId { get; }
		public Guid PeerId { get; }

		public NamedPipesClient()
			: this(Guid.NewGuid(), Guid.NewGuid())
		{
		}

		public NamedPipesClient(Guid ownId, Guid peerId)
		{
			OwnId = ownId;
			PeerId = peerId;

			_received = CreateReceiver();
		}

		private IObservable<object> CreateReceiver()
		{
			return Observable.Create<object>(async (observer, cancellationToken) =>
							{
								using (var pipe = CreateServerStream())
								{
									while (!cancellationToken.IsCancellationRequested)
									{
										await pipe.WaitForConnectionAsync(cancellationToken).ConfigureAwait(false);

										var message = ReadMessage(pipe);
										observer.OnNext(message);
										pipe.Disconnect();
									}
								}
							})
							.Publish()
							.RefCount();
		}

		private NamedPipeServerStream CreateServerStream()
		{
			return new NamedPipeServerStream(OwnId.ToString("N"), PipeDirection.In, 1, PipeTransmissionMode.Byte);
		}

		private NamedPipeClientStream CreateClientStream()
		{
			return new NamedPipeClientStream(".", PeerId.ToString("N"), PipeDirection.Out);
		}

		public IObservable<T> Received<T>()
		{
			return _received
					.Where(m => m is T)
					.Select(m => (T)m);
		}

		public async Task SendAsync(object message, CancellationToken cancellationToken = default)
		{
			using (var pipe = CreateClientStream())
			{
				await pipe.ConnectAsync(cancellationToken).ConfigureAwait(false);
				WriteMessage(pipe, message);
			}
		}

		private void WriteMessage(Stream stream, object message)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			using (var writer = new StreamWriter(stream))
			using (var jsonWriter = new JsonTextWriter(writer))
			{
				var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };
				var serializer = JsonSerializer.CreateDefault(settings);
				serializer.Serialize(jsonWriter, message);
			}
		}

		private object ReadMessage(Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException(nameof(stream));

			using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4096, true))
			using (var jsonReader = new JsonTextReader(reader))
			{
				var settings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects };
				var serializer = JsonSerializer.CreateDefault(settings);
				return serializer.Deserialize(jsonReader);
			}
		}
	}
}
