using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace StreamingBasics.Hubs
{
    public class AsyncEnumerableHub : Hub
    {
        public AsyncEnumerableHub()
        {
            
        }

        public async IAsyncEnumerable<int> GetNumbers(int count, int delay, [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            for(var i = 0; i<count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                yield return i;

                await Task.Delay(delay, cancellationToken);

            }
        }
    }
}
