using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ipc.NamedPipes;

namespace MainProcess
{
	class Program
	{
		static async Task Main()
		{
			Console.CancelKeyPress += (sender, args) => Environment.Exit(0);

			var processPool = new ChildProcessPool();

			while (true)
			{
				using (var childProcessLease = processPool.LeaseProcess())
				{
					var request = new RandomNextRequest() { Id = Guid.NewGuid() };
					var response = await childProcessLease.ChildProcess.ExecuteAsync<RandomNextResponse>(request).ConfigureAwait(false);

					Console.WriteLine($"[{DateTime.Now}] Next random number is {response.Value}");
				}

				Console.ReadLine();
			}
		}
	}
}
