using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Enums;
using API.Models;

namespace API.interfaces
{
    public interface IUserLikeRepository
    {
        Task<UserLike> GetUserLike(int sourceId,int targetId);
        Task<Users> GetUserWithLikes(int userId);
        Task<IEnumerable<LikeDto>> GetUserLikes(PredicateLikeEnum predicate,int userId);
        Task AddLike(int sourceId,int targetId);
        Task<bool> SaveAsync();
    }
}