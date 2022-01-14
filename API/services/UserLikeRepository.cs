using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Enums;
using API.interfaces;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.services
{
    public class UserLikeRepository : IUserLikeRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public UserLikeRepository(DataContext dataContext, IMapper mapper = null)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        public async Task AddLike(int sourceId, int targetId)
        {
            await _dataContext.UserLike.AddAsync(new UserLike(sourceId, targetId));
        }

        public async Task<UserLike> GetUserLike(int sourceId, int targetId)
        {
            return await _dataContext.UserLike.FindAsync(sourceId, targetId);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikes(PredicateLikeEnum predicate, int userId)
        {
            var users = _dataContext.Users.AsQueryable();
            var likes = _dataContext.UserLike.AsQueryable();
            if (predicate == PredicateLikeEnum.Liked)
            {
                //کاربر چه افرادی را لایک کرده است
                likes = likes.Include(x => x.TargetUser)
                    .ThenInclude(x => x.Photos)
                    .Where(x => x.SourceUserId == userId);
                users = likes.Select(x => x.TargetUser);
            }
            if (predicate == PredicateLikeEnum.LikeBy)
            {
                //چه افرادی من را لایک کرده اند
                likes = likes.Include(x => x.SourceUser)
                    .ThenInclude(x => x.Photos)
                    .Where(x => x.TargetUserId == userId);
                users = likes.Select(x => x.SourceUser);
            }
            return (await users.ToListAsync())
            .Select(x => _mapper.Map<LikeDto>(x));
        }

        public async Task<Users> GetUserWithLikes(int userId)
        {
            return await _dataContext.Users.Include(x => x.TargetUserLikes)
            .FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}