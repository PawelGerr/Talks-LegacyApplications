using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ipc.NamedPipes
{
	public class Program
	{
		public static async Task Main()
		{
			Console.CancelKeyPress += (sender, args) => Environment.Exit(0);

			var peerA = new NamedPipesClient();
			var peerB = new NamedPipesClient(peerA.PeerId, peerA.OwnId);

			peerB.Received<DateTime>()
					.Subscribe(date =>
					{
						Console.WriteLine($"Peer B received: {date}. Sending Ok ...");
						peerB.SendAsync("Ok");
					});

			peerA.Received<string>()
				.Subscribe(message => Console.WriteLine($"Peer A received: {message}"));

			while (true)
			{
				var now = DateTime.Now;
				Console.WriteLine($"Peer A is sending: {now}");

				await peerA.SendAsync(now);

				Console.ReadLine();
			}
		}
	}
}
