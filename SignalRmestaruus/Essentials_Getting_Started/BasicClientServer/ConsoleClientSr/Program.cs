using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace ConsoleClientSr
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl("https://localhost:7142/accessorHub")
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

            Console.ReadLine(); // Keep the console open
        }
    }
}
