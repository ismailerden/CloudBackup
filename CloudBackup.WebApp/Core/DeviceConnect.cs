using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudBackup.WebApp.Core
{
    public class DeviceConnect : Hub
    {
        public override Task OnConnectedAsync()
        {
            return Clients.Client(Context.ConnectionId).SendAsync("SetConnectionId", Context.ConnectionId);
        }
        public Task SendMessage(string message)
        {
            return Clients.All.SendAsync("GetMessage", message);
        }
    }
}
