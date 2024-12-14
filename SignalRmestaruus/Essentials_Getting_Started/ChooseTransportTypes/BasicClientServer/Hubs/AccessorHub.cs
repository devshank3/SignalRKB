using Microsoft.AspNetCore.SignalR;

namespace BasicClientServer.Hubs
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

    }
}
