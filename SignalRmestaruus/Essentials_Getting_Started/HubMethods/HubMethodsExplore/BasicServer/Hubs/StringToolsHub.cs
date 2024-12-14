using Microsoft.AspNetCore.SignalR;

namespace BasicServer.Hubs
{
    public class StringToolsHub : Hub
    {
        public string GetFullName(string firstName, string lastName)
        {
            return $"{firstName} {lastName}";
        }

        public async Task<string> GetRandomString()
        {
            await Task.Delay(3000);

            return Guid.NewGuid().ToString();
        }

        public static int ViewCount { get; set; } = 0;

        public async Task NotifyWatching()
        {
            ViewCount++;

            // Notify all clients
            await Clients.All.SendAsync("UpdateViewCount", ViewCount);
        }

        
    }
}
