using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Helpers;
using API.Models;

namespace API.interfaces
{
    public interface IMessageRepository
    {
        //signalR
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        //end
        Task AddMessage(Message message);
        Task<Message> GetMessageById(int id);
        void DeleteMessage(Message message);
        //all message
        Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams);
        // message between two users
        Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientName);
        Task<bool> SaveAll();
        Task UpdateMessageToRead(List<MessageDto> messages, string userName);
    }
}