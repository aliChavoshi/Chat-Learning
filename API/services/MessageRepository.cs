using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Helpers;
using API.interfaces;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.services
{
    public class MessageRepository : IMessageRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public MessageRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddMessage(Message message)
        {
            await _context.Message.AddAsync(message);
        }

        public void DeleteMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public async Task<Message> GetMessageById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedList<MessageDto>> GetMessageForUser(MessageParams messageParams)
        {
            //inbox , outbox , unread
            var query = _context.Message.OrderByDescending(x => x.MessageSent).AsQueryable();
            query = messageParams.Container switch
            {
                "Inbox" => query.Where(x => x.ReceiverUserName == messageParams.UserName),
                "Outbox" => query.Where(x => x.SenderUserName == messageParams.UserName),
                _ => query.Where(x => x.ReceiverUserName == messageParams.UserName && !x.DateRead.HasValue) //unread message
            };
            var messages = query.ProjectTo<MessageDto>(_mapper.ConfigurationProvider);
            return await PagedList<MessageDto>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);
        }

        public async Task<IEnumerable<MessageDto>> GetMessageThread(string currentUserName, string recipientName)
        {
            var query = await _context.Message.Where(
                x => x.ReceiverUserName == currentUserName && x.SenderUserName == recipientName ||
                x.SenderUserName == currentUserName && x.ReceiverUserName == recipientName).OrderBy(x => x.MessageSent).ToListAsync();
            await UpdateMessageToRead(query, currentUserName);
            return
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task UpdateMessageToRead(List<Message> messages, string userName)
        {
            messages = messages.Where(x => !x.DateRead.HasValue && x.ReceiverUserName == userName).ToList(); //دریافت ککنده خودم باشم
            messages.ForEach(x =>
            {
                x.DateRead = DateTime.Now;
                x.IsRead = true;
            });
            _context.UpdateRange(messages);
            await _context.SaveChangesAsync();
        }
    }
}