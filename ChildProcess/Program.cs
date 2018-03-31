using System;
using System.Diagnostics;
using Ipc.NamedPipes;

namespace ChildProcess
{
	public class Program
	{
		static void Main(string[] args)
		{
			Console.CancelKeyPress += (sender, _) => Environment.Exit(0);

			var mainIpcId = new Guid(args[0]);
			var ownIpcId = new Guid(args[1]);
			var parentProcessId = Int32.Parse(args[2]);

			TerminateOnParentExit(parentProcessId);

			var ipcClient = new NamedPipesClient(ownIpcId, mainIpcId);
			var random = new Random();

			ipcClient.Received<RandomNextRequest>()
					.Subscribe(async req =>
					{
						Console.WriteLine($"[{DateTime.Now}] Request received.");

						var value = random.Next();
						var response = new RandomNextResponse(req.Id, value);
						await ipcClient.SendAsync(response).ConfigureAwait(false);
					});

			Console.ReadLine();
		}

		private static void TerminateOnParentExit(int parentProcessId)
		{
			var parentProcess = Process.GetProcessById(parentProcessId);

			parentProcess.EnableRaisingEvents = true;
			parentProcess.Exited += (sender, args) => Environment.Exit(-1);

			if (parentProcess.HasExited)
				Environment.Exit(-1);
		}
	}
}
