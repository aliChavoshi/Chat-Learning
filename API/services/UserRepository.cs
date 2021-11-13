using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.interfaces;

namespace API.services
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public Task<IEnumerable<Users>> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Update(Users user)
        {
            throw new NotImplementedException();
        }
    }
}