using System;
using System.Data;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;

namespace Ipc.NamedPipes
{
   public class NamedPipesRequestResponseClient
   {
      private static readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(5);

      private readonly NamedPipesClient _ipcClient;

      public Guid OwnId => _ipcClient.OwnId;
      public Guid PeerId => _ipcClient.PeerId;

      public NamedPipesRequestResponseClient(NamedPipesClient ipcClient)
      {
         _ipcClient = ipcClient ?? throw new ArgumentNullException(nameof(ipcClient));
      }

      public async Task<TResponse> ExecuteAsync<TResponse>(IRequest request, TimeSpan? timeout = null, CancellationToken cancellationToken = default)
         where TResponse : IResponse
      {
         var responseTask = _ipcClient.Received<TResponse>()
                                      .Where(resp => resp.RequestId == request.Id)
                                      .Timeout(timeout ?? _defaultTimeout)
                                      .FirstOrDefaultAsync()
                                      .ToTask(cancellationToken);

         await _ipcClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

         var response = await responseTask.ConfigureAwait(false);

         if (response == null)
            throw new DataException("Server didn't yield any result.");

         return response;
      }
   }
}
