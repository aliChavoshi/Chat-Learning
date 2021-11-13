using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Errors;
using API.interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<MemberDto>> GetUsers()
        {
            var users = await _userRepository.GetAllUsers();
            return users.Select(x => new MemberDto()
            {
                Birthday = x.Birthday,
                City = x.City
            });
        }

        [HttpGet("{id:int}")]
        // [Authorize]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return BadRequest(new ApiResponse(400, "کاربری یافت نشد"));
            return user;
        }
    }
}