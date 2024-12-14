using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;

namespace ConsoleClient
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7208/accessorHub", options =>
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


            connection.Closed += async (error) =>
            {
                Console.WriteLine("Connection closed. Trying to restart...");
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };


            connection.Reconnecting += error =>
            {
                Console.WriteLine("Connection lost. Reconnecting...");
                return Task.CompletedTask;
            };


            connection.Reconnected += connectionId =>
            {
                Console.WriteLine($"Reconnected. ConnectionId: {connectionId}");
                return Task.CompletedTask;
            };

            connection.On<int>("UpdateViewCount", (viewCount) =>
            {
                Console.WriteLine($"View count: {viewCount}");
            });

            try
            {
                connection.StartAsync().Wait();
                Console.WriteLine("Connection started successfully.");
                await connection.InvokeAsync("NotifyWatching");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }

            while (true)
            {
                Console.WriteLine("Enter 'r' to get a random string from the server or 'e' to exit:");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "r":
                        try
                        {
                            var randomString = await connection.InvokeAsync<string>("GetRandomString");
                            Console.WriteLine($"Random string from server: {randomString}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error calling GetRandomString: {ex.Message}");
                        }
                        break;

                    case "e":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please enter 'r' or 'e'.");
                        break;
                }
            }
            
        }
    }
}
