using System;
using System.Diagnostics;
using System.IO;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using Ipc.NamedPipes;

namespace MainProcess
{
	public class ChildProcess : IDisposable
	{
		private readonly NamedPipesClient _ipcClient;
		private readonly Process _process;

		public bool HasExited => _process.HasExited;

		public ChildProcess(NamedPipesClient ipcClient)
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

		public async Task<TResponse> ExecuteAsync<TResponse>(IRequest request, CancellationToken cancellationToken = default)
			where TResponse : IResponse
		{
			var responseTask = _ipcClient.Received<TResponse>()
										.Where(resp => resp.RequestId == request.Id)
										.Timeout(TimeSpan.FromSeconds(5))
										.FirstOrDefaultAsync()
										.ToTask(cancellationToken);

			await _ipcClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

			return await responseTask.ConfigureAwait(false);
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
