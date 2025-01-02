using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.SignalR;

namespace StreamingBasics.Hubs
{
    public class AsyncEnumerableHub : Hub
    {
        public AsyncEnumerableHub()
        {
            
        }

        public record WeatherData(int temp, int air, DateTime TimeStamp);


        public async IAsyncEnumerable<int> GetNumbers(int count, int delay, [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            for(var i = 0; i<count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                yield return i;

                await Task.Delay(delay, cancellationToken);

            }
        }

        public async IAsyncEnumerable<WeatherData> GetWeatherData(int count, int delay, [EnumeratorCancellation]CancellationToken cancellationToken)
        {
            Random random = new Random();

            for(var i = 0; i<count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                yield return new WeatherData(random.Next(24,34), random.Next(3000,5000), DateTime.Now);

                await Task.Delay(delay, cancellationToken);

            }
        }
    }
}
