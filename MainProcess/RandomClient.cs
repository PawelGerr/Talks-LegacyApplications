using System;
using System.Threading.Tasks;
using Ipc.NamedPipes;

namespace MainProcess
{
	public class RandomClient
	{
		private readonly ChildProcessPool _processPool;

		public RandomClient(ChildProcessPool processPool)
		{
			_processPool = processPool ?? throw new ArgumentNullException(nameof(processPool));
		}

		public async Task<int> NextAsync()
		{
			using (var childProcessLease = _processPool.LeaseProcess())
			{
				var request = new RandomNextRequest() { Id = Guid.NewGuid() };
				var response = await childProcessLease.ChildProcess.ExecuteAsync<RandomNextResponse>(request).ConfigureAwait(false);

				return response.Value;
			}
		}
	}
}
