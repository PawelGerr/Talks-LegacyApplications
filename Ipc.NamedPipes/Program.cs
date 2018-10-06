using System;
using System.Threading.Tasks;

namespace Ipc.NamedPipes
{
	public class Program
	{
		public static async Task Main()
		{
			Console.CancelKeyPress += (sender, args) => Environment.Exit(0);

			await NamedPipesClientDemo().ConfigureAwait(false);
			//await RandomNextDemo().ConfigureAwait(false);
		}

		private static async Task NamedPipesClientDemo()
		{
			var peerA = new NamedPipesClient();
			var peerB = new NamedPipesClient(peerA.PeerId, peerA.OwnId);

			peerB.Received<DateTime>()
				.Subscribe(date => Console.WriteLine($"Peer B received: {date}."));

			while (true)
			{
				var now = DateTime.Now;
				Console.WriteLine($"Peer A is sending: {now}");

				await peerA.SendAsync(now).ConfigureAwait(false);

				Console.ReadLine();
			}
		}

		private static async Task RandomNextDemo()
		{
			var serverIpc = new NamedPipesClient();
			var random = new Random();
			var server = new MyNativeLibServer(serverIpc, random);
			server.Start();

         // -----------------

			var clientIpc = new NamedPipesClient(serverIpc.PeerId, serverIpc.OwnId);
			var client = new NamedPipesRequestResponseClient(clientIpc);

         //-----------------

			while (true)
			{
				var request = new RandomNextRequest() { Id = Guid.NewGuid() };

				var response = await client.ExecuteAsync<RandomNextResponse>(request).ConfigureAwait(false);
				Console.WriteLine($"[{DateTime.Now}] Next random number is {response.Value}");

				Console.ReadLine();
			}
		}
	}
}
