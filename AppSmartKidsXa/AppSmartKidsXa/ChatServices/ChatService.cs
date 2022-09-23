using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppSmartKidsXa.ChatServices
{
    public  class ChatService 
    {
        private  readonly HubConnection hubConnection;
        public  ChatService()
        {
            try
            {
                hubConnection = new HubConnectionBuilder().WithUrl("http://smartserveriq-001-site5.htempurl.com/Signalr").Build();
            }
            catch { }
        }
        public async Task connect()
        {
            var cts = new CancellationTokenSource(5000);
            await hubConnection.StartAsync(cts.Token); 
        }
        public  async Task SendMessage( string message,string func)
        { 
             await hubConnection.InvokeAsync(func,  message); 
        }
        public async Task DisConnect()
        {
            await hubConnection.StopAsync();
        }
    }
}
