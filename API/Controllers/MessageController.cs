using System.Reflection.Metadata.Ecma335;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Errors;
using API.extensions;
using API.interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AutoMapper;
using API.Helpers;

namespace API.Controllers
{
    [Authorize]
    public class MessageController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;
        public MessageController(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDto>> CreateMessage(CreateMessageDto createMessage)
        {
            var currentUser = User.GetUserName();
            if (currentUser == createMessage.RecipientUserName) return BadRequest(new ApiResponse(400, "You cannot send message to yourself"));
            var sender = await _uow.UserRepository.GetUserByUserNameWithPhotos(currentUser);
            if (sender == null) return BadRequest(new ApiResponse(404, "Sender not found"));
            var recipient = await _uow.UserRepository.GetUserByUserNameWithPhotos(createMessage.RecipientUserName);
            if (recipient == null) return BadRequest(new ApiResponse(404, "Recipient not found"));
            var message = new Message
            {
                SenderId = sender.Id,
                SenderUserName = sender.UserName,
                ReceiverId = recipient.Id,
                ReceiverUserName = recipient.UserName,
                Content = createMessage.Content,
            };
            await _uow.MessageRepository.AddMessage(message);
            if (await _uow.CompleteAsync())
                return Ok(_mapper.Map<Message, MessageDto>(message));
            return BadRequest(new ApiResponse(400, "Failed to send message"));
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<MessageDto>>> GetMessages([FromQuery] MessageParams messageParams)
        {
            var currnetUserName = User.GetUserName();
            messageParams.UserName = currnetUserName;
            return Ok(await _uow.MessageRepository.GetMessageForUser(messageParams));
        }

        [HttpGet("thread/{UserName}")]
        public async Task<ActionResult<IEnumerable<MessageDto>>> GetMessageThread(string userName)
        {
            var currnetUserName = User.GetUserName();
            return Ok(await _uow.MessageRepository.GetMessageThread(currnetUserName, userName));
        }
    }
}