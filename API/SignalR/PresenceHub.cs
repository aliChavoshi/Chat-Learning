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
        private readonly PresenceTracker _tracker;

        public PresenceHub(PresenceTracker tracker)
        {
            _tracker = tracker;
        }

        //connect to hub 
        public override async Task OnConnectedAsync()
        {
            await _tracker.UserConnected(Context.User.GetUserName(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOnline", Context.User.GetUserName()); //send to all except me
            //UserIsOnline > Method
            //tracker get online users
            await GetOnlineUsers();
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await _tracker.UserDisconnected(Context.User.GetUserName(), Context.ConnectionId);
            await Clients.Others.SendAsync("UserIsOffline", Context.User.GetUserName()); //send notification
            //tracker get online users
            await GetOnlineUsers();

            await base.OnDisconnectedAsync(exception);
        }
        private async Task GetOnlineUsers()
        {
            var currentUsers = await _tracker.GetOnlineUsers();
            await Clients.All.SendAsync("GetOnlineUsers", currentUsers);
        }
    }
}