using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Enums;
using API.interfaces;
using API.Models;

namespace API.services
{
    public class UserLikeRepository : IUserLikeRepository
    {
        private readonly DataContext _dataContext;

        public UserLikeRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddLike(int sourceId, int targetId)
        {
            await _dataContext.UserLike.AddAsync(new UserLike(sourceId,targetId));
        }

        public async Task<UserLike> GetUserLike(int sourceId, int targetId)
        {
            return await _dataContext.UserLike.FindAsync(sourceId,targetId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(PredicateLikeEnum predicate, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Users> GetUserWithLikes(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}