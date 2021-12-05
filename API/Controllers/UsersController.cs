using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Errors;
using API.interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            return Ok(await _userRepository.GetAllUsersMemberDto());
        }

        [HttpGet("getUserById/{id:int}")]
        public async Task<ActionResult<MemberDto>> GetUserById(int id)
        {
            var user = await _userRepository.GetMemberDtoById(id);
            if (user == null) return NotFound(new ApiResponse(404, "چنین کاربری یافت نشد"));
            return Ok(user);
        }

        [HttpGet("getUserByUserName/{userName}")]
        public async Task<ActionResult<MemberDto>> GetUserByUserName(string userName)
        {
            var user = await _userRepository.GetMemberDtoByUserName(userName);
            if (user == null) return NotFound(new ApiResponse(404, "چنین کاربری یافت نشد"));
            return Ok(user);
        }

        [HttpPut("UpdateUser")]
        [Authorize] // header => JWT
        public async Task<ActionResult<MemberDto>> UpdateUser([FromBody] MemberUpdateDto memberDto)
        {
            var username = HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var member = await _userRepository.GetUserByUserNameWithPhotos(username);
            if (member == null) return NotFound(new ApiResponse(404));

            member = _mapper.Map(memberDto, member);
            _userRepository.Update(member);
            if (await _userRepository.SaveAllAsync())
                return Ok(_mapper.Map<MemberDto>(member));

            return BadRequest(new ApiResponse(400));
        }
    }
}