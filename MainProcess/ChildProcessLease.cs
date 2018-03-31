using System;
using System.Threading.Tasks;
using Ipc.NamedPipes;

namespace MainProcess
{
	public class ChildProcessLease : IDisposable
	{
		private readonly ChildProcessPool _pool;
		public ChildProcess ChildProcess { get; }

		public ChildProcessLease(ChildProcessPool pool, ChildProcess childProcess)
		{
			_pool = pool ?? throw new ArgumentNullException(nameof(pool));
			ChildProcess = childProcess ?? throw new ArgumentNullException(nameof(childProcess));
		}

		public void Dispose()
		{
			_pool.Reclaim(ChildProcess);
		}
	}
}
