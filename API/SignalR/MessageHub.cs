using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.extensions;
using API.interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _message;
        private readonly IMapper _mapper;

        public MessageHub(IMessageRepository message, IMapper mapper)
        {
            _message = message;
            _mapper = mapper;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var currentUser = Context.User.GetUserName();
            //create group name
            var groupName = CreateGroupName(currentUser, otherUser);
            //create group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //get all message between user
            var messages = await _message.GetMessageThread(currentUser, otherUser);
            //send message to user group
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        private static string CreateGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0; //caller >  other => true
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }
    }
}