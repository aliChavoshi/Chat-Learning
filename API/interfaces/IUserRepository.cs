using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Models;

namespace API.interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<Users>> GetAllUsers();
        Task<IEnumerable<MemberDto>> GetAllUsersMemberDto();
        Task<Users> GetUserById(int id);
        Task<MemberDto> GetMemberDtoById(int userId);
        Task<MemberDto> GetMemberDtoByUserName(string userName);
        Task<Users> GetUserByUserName(string userName);
        Task<Users> GetUserByUserNameWithPhotos(string userName);
        void Update(Users user);
        Task<bool> SaveAllAsync();
    }
}