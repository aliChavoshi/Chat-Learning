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
        /* Creating a new instance of the MessageHub class. */
        private readonly IMapper _mapper;
        private readonly PresenceTracker _tracker;
        private readonly IHubContext<PresenceHub> _presence;
        private readonly IUnitOfWork _uow;
        public MessageHub(IMapper mapper, PresenceTracker tracker, IHubContext<PresenceHub> presence, IUnitOfWork uow)
        {
            _mapper = mapper;
            _tracker = tracker;
            _presence = presence;
            _uow = uow;
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
            var messages = await _uow.MessageRepository.GetMessageThread(currentUser, otherUser);
            //change Tracker
            if (_uow.HasChanges()) await _uow.CompleteAsync();
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
            var sender = await _uow.UserRepository.GetUserByUserNameWithPhotos(currentUser);
            if (sender == null) throw new HubException("Sender not found");
            var recipient = await _uow.UserRepository.GetUserByUserNameWithPhotos(model.RecipientUserName);
            if (recipient == null) throw new HubException("Sender not found");
            //create new message
            var message = new Message
            {
                SenderId = sender.Id,
                SenderUserName = sender.UserName,
                ReceiverId = recipient.Id,
                ReceiverUserName = recipient.UserName,
                Content = model.Content,
            };
            await _uow.MessageRepository.AddMessage(message);
            //create groupName
            var groupName = GetGroupName(currentUser, recipient.UserName);
            //check user exist in group
            var group = await _uow.MessageRepository.GetMessageGroup(groupName);
            if (group.Connections.Any(x => x.UserName == recipient.UserName))
            {
                message.DateRead = DateTime.Now;
                message.IsRead = true;
            }
            else
            {
                //send notification
                //online users
                var connectionIds = await _tracker.GetConnectionsForUser(recipient.UserName);
                if (connectionIds != null)
                {
                    await _presence.Clients.Clients(connectionIds).SendAsync("NewMessageReceived",
                        new { userName = sender.UserName, content = model.Content });
                }
            }
            //add new message
            if (await _uow.CompleteAsync())
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
            var group = await _uow.MessageRepository.GetMessageGroup(groupName);
            var connection = new Connection(context.ConnectionId, context.User.GetUserName());
            if (group == null)
            {
                group = new Group(groupName);
                _uow.MessageRepository.AddGroup(group);
            }
            group.Connections.Add(connection);
            return await _uow.CompleteAsync();
        }
        private async Task RemoveFromMessageGroup(string connectionId)
        {
            var connection = await _uow.MessageRepository.GetConnection(connectionId);
            _uow.MessageRepository.RemoveConnection(connection);
            await _uow.CompleteAsync();
        }
    }
}