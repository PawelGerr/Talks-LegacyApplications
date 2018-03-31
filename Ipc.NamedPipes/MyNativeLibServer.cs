using System;
using System.Threading;

namespace Ipc.NamedPipes
{
	public class MyNativeLibServer
	{
		private readonly NamedPipesClient _ipcClient;
		private readonly Random _random;

		public MyNativeLibServer(NamedPipesClient ipcClient, Random random)
		{
			_ipcClient = ipcClient ?? throw new ArgumentNullException(nameof(ipcClient));
			_random = random ?? throw new ArgumentNullException(nameof(random));
		}

		public void Start(CancellationToken cancellationToken = default)
		{
			_ipcClient.Received<RandomNextRequest>()
					.Subscribe(async req =>
					{
						var value = _random.Next();
						var response = new RandomNextResponse(req.Id, value);
						await _ipcClient.SendAsync(response, cancellationToken).ConfigureAwait(false);
					}, cancellationToken);
		}
	}
}
