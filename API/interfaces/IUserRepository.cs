using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<Users> GetUserById(int id);
        Task<Users> GetUserByUserName(string userName);
        void Update(Users user);
        Task<bool> SaveAllAsync();
    }
}