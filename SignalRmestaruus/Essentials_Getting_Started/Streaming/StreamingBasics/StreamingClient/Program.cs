using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace StreamingClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7127/nstream", options =>
                {
                    //options.Transports = HttpTransportType.WebSockets ;
                    options.Transports = HttpTransportType.WebSockets | HttpTransportType.ServerSentEvents;
                })
                .ConfigureLogging(
                    logging =>
                    {
                        logging.AddConsole(); // This requires Microsoft.Extensions.Logging.Console package

                        // This will set ALL logging to Debug level
                        logging.SetMinimumLevel(LogLevel.Debug);

                    })
                .Build();

            try
            {
                connection.StartAsync().Wait();
                Console.WriteLine("Connection started successfully.");
                var cancellationTokenSource = new CancellationTokenSource();

                var stream = connection.StreamAsync<int>(
                    "GetNumbers", 10, 5000, cancellationTokenSource.Token);


                await foreach (var count in stream)
                {
                    Console.WriteLine($"{count}");
                }

                Console.WriteLine("Streaming completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
