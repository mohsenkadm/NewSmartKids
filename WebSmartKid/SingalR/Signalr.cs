using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSmartKid
{
    public class Signalr : Hub
    {
        // Track connected users for targeted messaging
        private static readonly ConcurrentDictionary<string, string> ConnectedUsers = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            var userId = Context.GetHttpContext()?.Session.GetString("Id");
            if (!string.IsNullOrEmpty(userId))
            {
                ConnectedUsers.TryAdd(Context.ConnectionId, userId);
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectedUsers.TryRemove(Context.ConnectionId, out _);
            await base.OnDisconnectedAsync(exception);
        }

        // Send message to all chat participants
        public async Task GetMessage(string message)
        {
            await Clients.All.SendAsync("OnGetMessage", message);
        }

        // Send message notification to specific user
        public async Task SendMessageToUser(string userId, string message)
        {
            var connections = ConnectedUsers.Where(x => x.Value == userId).Select(x => x.Key).ToList();
            if (connections.Any())
            {
                await Clients.Clients(connections).SendAsync("OnGetMessage", message);
            }
        }

        // Broadcast order notification
        public async Task GetOrder(string message)
        {
            await Clients.All.SendAsync("GetOrder", message);
        }

        // Typing indicator for chat
        public async Task UserTyping(string userId, bool isTyping)
        {
            await Clients.Others.SendAsync("OnUserTyping", userId, isTyping);
        }
    }
}
