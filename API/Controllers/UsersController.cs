using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Errors;
using API.interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(await _userRepository.GetAllUsersMemberDto());
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