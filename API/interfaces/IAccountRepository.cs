using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.interfaces
{
    public interface IAccountRepository
    {
        Task<bool> IsExistUserName(string userName);
        Task AddUser(Users user);
        Task<Users> GetUserByUserName(string userName);
        Task<Users> GetUserByUserNameWithPhotos(string userName);
        Task<bool> SaveChangeAsync();
    }
}