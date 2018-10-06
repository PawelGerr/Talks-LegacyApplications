using System;
using System.Collections.Generic;
using Ipc.NamedPipes;

namespace MainProcess
{
	public class ChildProcessPool
	{
		private readonly Queue<ChildProcess> _freeProcesses;
		private readonly object _lock;

		public ChildProcessPool()
		{
			_lock = new object();
			_freeProcesses = new Queue<ChildProcess>();
		}

		public ChildProcessLease LeaseProcess()
		{
			ChildProcess childProcess;

			lock (_lock)
			{
				_freeProcesses.TryDequeue(out childProcess);
			}

			if (childProcess == null)
				childProcess = SpawnNewProcess();

			return new ChildProcessLease(this, childProcess);
		}

		private ChildProcess SpawnNewProcess()
		{
			var ipcClient = new NamedPipesClient();
			var requestResponseClent = new NamedPipesRequestResponseClient(ipcClient);
			var childProcess = new ChildProcess(requestResponseClent);

			childProcess.Start();

			return childProcess;
		}

		public void Reclaim(ChildProcess childProcess)
		{
			if (childProcess.HasExited)
				return;

			lock (_lock)
			{
				_freeProcesses.Enqueue(childProcess);
			}
		}
	}
}
