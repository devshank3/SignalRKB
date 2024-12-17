using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace ColorConsole
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7039/ColorProvider", options =>
                {
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

            connection.On<string>("TriggerColor", groupName =>
            {
                switch (groupName)
                {
                    case "Red":
                        Console.BackgroundColor = ConsoleColor.Red;
                        break;
                    case "Blue":
                        Console.BackgroundColor = ConsoleColor.Blue;
                        break;
                    case "Green":
                        Console.BackgroundColor = ConsoleColor.Green;
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.Black;
                        break;
                }

                Console.Clear();
                Console.WriteLine($"Background color changed for group: {groupName}");
            });

            try
            {
                await connection.StartAsync();
                Console.WriteLine("Connection started successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection failed: {ex.Message}");
            }

            while (true)
            {
                Console.WriteLine("Enter '1' to Join a Group, '2' to Trigger background color, or 'e' to exit:");
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Enter group name to join (Red, Blue, Green):");
                        var groupName = Console.ReadLine();
                        if (IsValidGroupName(groupName))
                        {
                            await JoinGroup(connection, groupName);
                        }
                        else
                        {
                            Console.WriteLine("Invalid group name. Please enter Red, Blue, or Green.");
                        }
                        break;

                    case "2":
                        Console.WriteLine("Enter group name to trigger color (Red, Blue, Green):");
                        var triggerGroupName = Console.ReadLine();
                        if (IsValidGroupName(triggerGroupName))
                        {
                            await TriggerGroup(connection, triggerGroupName);
                        }
                        else
                        {
                            Console.WriteLine("Invalid group name. Please enter Red, Blue, or Green.");
                        }
                        break;

                    case "e":
                        Console.WriteLine("Exiting...");
                        return;

                    default:
                        Console.WriteLine("Invalid input. Please try again.");
                        break;
                }
            }
        }

        private static bool IsValidGroupName(string groupName)
        {
            return groupName == "Red" || groupName == "Blue" || groupName == "Green";
        }

        private static async Task JoinGroup(HubConnection connection, string groupName)
        {
            await connection.InvokeAsync("JoinGroup", groupName);
            Console.WriteLine($"Joined group: {groupName}");
        }

        private static async Task TriggerGroup(HubConnection connection, string groupName)
        {
            await connection.InvokeAsync("TriggerGroup", groupName);
            Console.WriteLine($"Triggered color change for group: {groupName}");
        }
    }
}
