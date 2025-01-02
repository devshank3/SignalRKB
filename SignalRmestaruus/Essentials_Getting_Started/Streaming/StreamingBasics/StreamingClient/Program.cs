using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace StreamingClient
{
    internal class Program
    {
        public record WeatherData(int temp, int air, DateTime TimeStamp);

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
                        logging.SetMinimumLevel(LogLevel.None);

                    })
                .Build();

            try
            {
                connection.StartAsync().Wait();
                Console.WriteLine("Connection started successfully.");
                var cancellationTokenSource = new CancellationTokenSource();

                Console.WriteLine("Streaming Numbers, is funnnn!!");

                var stream = connection.StreamAsync<int>(
                    "GetNumbers", 10, 2000, cancellationTokenSource.Token);


                await foreach (var count in stream)
                {
                    Console.WriteLine($"{count}");
                }

                Console.WriteLine("\n\nStreaming weather is less fun !!!!");

                var weatherStream = connection.StreamAsync<WeatherData>(
                    "GetWeatherData", 10, 2000, cancellationTokenSource.Token);

                await foreach (var weather in weatherStream)
                {
                    Console.WriteLine($"Temp: {weather.temp}, Air: {weather.air}, Time: {weather.TimeStamp}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            Console.ReadLine();
        }
    }
}
