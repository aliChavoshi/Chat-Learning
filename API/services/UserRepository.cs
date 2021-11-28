using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
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

        public async Task<IEnumerable<MemberDto>> GetAllUsersMemberDto()
        {
            return await _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();
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