using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSmartKid
{
    public class Signalr : Hub
    {
        public Task GetMessage(string message)
        {
            return Clients.All.SendAsync("OnGetMessage", message);
        }
        public Task GetOrder(string message)
        {
            return Clients.All.SendAsync("GetOrder", message);
        }                        
    }
}
