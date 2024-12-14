using Microsoft.AspNetCore.SignalR;

namespace BasicServer.Hubs
{
    public class AccessorHub : Hub
    {
        public static int ViewCount { get; set; } = 0;

        public async Task NotifyWatching()
        {
            ViewCount++;

            // Notify all clients
            await Clients.All.SendAsync("UpdateViewCount", ViewCount);
        }

        public string GetRandomString()
        {
            return Guid.NewGuid().ToString();
        }

    }
}
