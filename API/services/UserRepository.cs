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
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Users>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<PagedList<MemberDto>> GetAllUsersMemberDto(UserParams userParams)
        {
            var query = _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking();
            query = query.Where(x => x.UserName != userParams.currentUserName);
            query = query.Where(x => x.Gender == userParams.Gender);
            //x1 < m < x2
            var minDate = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDate = DateTime.Today.AddYears(-userParams.MinAge);
            query = query.Where(x => x.DateOfBirth.Date >= minDate.Date && x.DateOfBirth.Date <= maxDate.Date);
            var items = await PagedList<MemberDto>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);
            return items;
        }

        public async Task<MemberDto> GetMemberDtoById(int userId)
        {
            return await _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Id == userId);
        }
        public async Task<MemberDto> GetMemberDtoByUserName(string userName)
        {
            return await _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

        public async Task<Users> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<Users> GetUserByUserName(string userName)
        {
            return await _context.Users
                .SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

        public async Task<Users> GetUserByUserNameWithPhotos(string userName)
        {
            return await _context.Users.Include(x => x.Photos)
                .SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Users user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}