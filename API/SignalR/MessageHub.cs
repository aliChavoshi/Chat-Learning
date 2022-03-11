using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.extensions;
using API.interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace API.SignalR
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _message;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public MessageHub(IMessageRepository message, IMapper mapper, IUserRepository userRepository)
        {
            _message = message;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var otherUser = httpContext.Request.Query["user"].ToString();
            var currentUser = Context.User.GetUserName();
            //create group name
            var groupName = GetGroupName(currentUser, otherUser);
            //add connection to group in Db
            await AddToGroupWithConnections(Context, groupName);
            //create group
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            //get all message between user
            var messages = await _message.GetMessageThread(currentUser, otherUser);
            //send message to user group
            await Clients.Group(groupName).SendAsync("ReceiveMessageThread", messages);
        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //remove connection from group in Db
            await RemoveFromMessageGroup(Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
        public async Task SendMessage(CreateMessageDto model)
        {
            var currentUser = Context.User.GetUserName();
            if (currentUser == model.RecipientUserName) throw new HubException("You cannot send message to yourself");
            var sender = await _userRepository.GetUserByUserNameWithPhotos(currentUser);
            if (sender == null) throw new HubException("Sender not found");
            var recipient = await _userRepository.GetUserByUserNameWithPhotos(model.RecipientUserName);
            if (recipient == null) throw new HubException("Sender not found");
            var message = new Message
            {
                SenderId = sender.Id,
                SenderUserName = sender.UserName,
                ReceiverId = recipient.Id,
                ReceiverUserName = recipient.UserName,
                Content = model.Content,
            };
            //create groupName
            var groupName = GetGroupName(currentUser, recipient.UserName);
            //check user exist in group
            var group = await _message.GetMessageGroup(groupName);
            if (group.Connections.Any(x => x.UserName == recipient.UserName))
            {
                message.DateRead = DateTime.Now;
                message.IsRead = true;
            }
            //add new message
            await _message.AddMessage(message);
            if (await _message.SaveAll())
            {
                await Clients.Group(groupName).SendAsync("NewMessage", _mapper.Map<MessageDto>(message));
            }
        }
        private static string GetGroupName(string caller, string other)
        {
            var stringCompare = string.CompareOrdinal(caller, other) < 0; //caller >  other => true
            return stringCompare ? $"{caller}-{other}" : $"{other}-{caller}";
        }

        private async Task<bool> AddToGroupWithConnections(HubCallerContext context, string groupName)
        {
            var group = await _message.GetMessageGroup(groupName);
            var connection = new Connection(context.ConnectionId, context.User.GetUserName());
            if (group == null)
            {
                group = new Group(groupName);
                _message.AddGroup(group);
            }
            group.Connections.Add(connection);
            return await _message.SaveAll();
        }

        private async Task RemoveFromMessageGroup(string connectionId)
        {
            var connection = await _message.GetConnection(connectionId);
            _message.RemoveConnection(connection);
            await _message.SaveAll();
        }
    }
}