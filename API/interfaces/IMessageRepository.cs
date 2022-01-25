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
        Task AddMessage(Message message);
        Task<Message> GetMessageById(int id);
        void DeleteMessage(Message message);
        //all message
        Task<PagedList<MessageDto>> GetMessageForUser();
        // message between two users
        Task<IEnumerable<MessageDto>> GetMessageThread(int currentUserId, int recipientId);
        Task<bool> SaveAll();
    }
}