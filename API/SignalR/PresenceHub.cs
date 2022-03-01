using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize] //token ->Context ->UserName
    public class PresenceHub : Hub
    {
        //connect to hub 
        public override async Task OnConnectedAsync()
        {
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName()); //send to all except me
            //UserIsOnline > Method
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName());
            await base.OnDisconnectedAsync(exception);
        }
    }
}