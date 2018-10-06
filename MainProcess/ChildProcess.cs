using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ipc.NamedPipes;

namespace MainProcess
{
	public class ChildProcess : IDisposable
	{
		private readonly NamedPipesRequestResponseClient _ipcClient;
		private readonly Process _process;

		public bool HasExited => _process.HasExited;

		public ChildProcess(NamedPipesRequestResponseClient ipcClient)
		{
			_ipcClient = ipcClient;
			_process = CreateProcess();
		}

		private Process CreateProcess()
		{
			var process = new Process()
			{
				StartInfo = CreateStartInfo(),
				EnableRaisingEvents = true
			};

			process.Exited += (sender, args) => Dispose();
			process.ErrorDataReceived += (sender, args) => Console.Error.WriteLine(args.Data);
			process.OutputDataReceived += (sender, args) => Console.Out.WriteLine(args.Data);

			return process;
		}

		private ProcessStartInfo CreateStartInfo()
		{
			var childProcessExe = Path.GetFullPath(@"..\..\..\..\ChildProcess\bin\netcoreapp2.1\ChildProcess.dll");
			var args = $"\"{childProcessExe}\" \"{_ipcClient.OwnId}\" \"{_ipcClient.PeerId}\" \"{Process.GetCurrentProcess().Id}\"";

			return new ProcessStartInfo("dotnet.exe", args)
			{
				UseShellExecute = false,
				RedirectStandardOutput = true,
				RedirectStandardError = true,
				RedirectStandardInput = true,
				CreateNoWindow = true,
				ErrorDialog = false,
				LoadUserProfile = false
			};
		}

		public void Start()
		{
			_process.Start();
			_process.BeginOutputReadLine();
			_process.BeginErrorReadLine();
		}

		public Task<TResponse> ExecuteAsync<TResponse>(IRequest request, CancellationToken cancellationToken = default)
			where TResponse : IResponse
		{
			return _ipcClient.ExecuteAsync<TResponse>(request, TimeSpan.FromSeconds(5), cancellationToken);
		}

		public void Dispose()
		{
			TerminateProcess();
			_process.Dispose();
		}

		private void TerminateProcess()
		{
			if (_process.HasExited)
				return;

			_process.Kill();
			_process.WaitForExit();
		}
	}
}
