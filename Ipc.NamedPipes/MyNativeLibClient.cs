using System;
using System.Data;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Ipc.NamedPipes
{
	public class MyNativeLibClient
	{
		private readonly NamedPipesClient _ipcClient;

		public MyNativeLibClient(NamedPipesClient ipcClient)
		{
			_ipcClient = ipcClient ?? throw new ArgumentNullException(nameof(ipcClient));
		}

		public async Task<int> NextAsync(CancellationToken cancellationToken = default)
		{
			var request = new RandomNextRequest() { Id = Guid.NewGuid() };

			var responseTask = _ipcClient.Received<RandomNextResponse>()
										.Where(resp => resp.RequestId == request.Id)
										.Timeout(TimeSpan.FromSeconds(5))
										.FirstOrDefaultAsync()
										.ToTask(cancellationToken);

			await _ipcClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

			var response = await responseTask.ConfigureAwait(false);

         if(response == null)
            throw new DataException("Server didn't yield any result.");

			return response.Value;
		}
	}
}
