using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.interfaces
{
    public interface IUnitOfWork
    {
        IMessageRepository MessageRepository { get; }
        IUserRepository UserRepository { get; }
        IUserLikeRepository UserLikeRepository { get; }
        Task<bool> CompleteAsync();
        bool HasChanges();
    }
}