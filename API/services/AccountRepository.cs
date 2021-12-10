using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.services
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataContext _context;

        public AccountRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddUser(Users user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<Users> GetUserByUserName(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

        public async Task<Users> GetUserByUserNameWithPhotos(string userName)
        {
            return await _context.Users.Include(x => x.Photos).SingleOrDefaultAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

        public async Task<bool> IsExistUserName(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }

        public async Task<bool> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}