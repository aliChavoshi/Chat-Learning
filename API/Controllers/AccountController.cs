using System.Runtime.CompilerServices;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Errors;
using API.interfaces;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly UserManager<Users> _userManager;
        private readonly SignInManager<Users> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        public AccountController(ITokenService tokenService, IMapper mapper, UserManager<Users> userManager, SignInManager<Users> signInManager, RoleManager<Role> roleManager)
        {
            _tokenService = tokenService;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        /// <summary>
        /// Register a new  <see cref="RegisterDto"/>
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserTokenDto>> Register(RegisterDto model)
        {
            if (await IsExistUserName(model.UserName))
                return BadRequest(new ApiResponse(400, model.UserName + "یافت نشد"));

            var user = _mapper.Map<Users>(model);

            //add user 
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "خطا در ثبت اطلاعات"));
            //add role to user
            var roleResult = await _userManager.AddToRoleAsync(user, "member");
            if (!roleResult.Succeeded) return BadRequest(new ApiResponse(400, "خطا در ثبت اطلاعات"));
            return Ok(new UserTokenDto
            {
                userName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                Gender = user.Gender,
                KnownAs = user.KnownAs,
            });
        }

        /// <summary>
        /// Login a <see cref="LoginDto"/>
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.Users.Include(x => x.Photos).FirstOrDefaultAsync(x => x.UserName == model.userName);
            if (user == null) return BadRequest(new ApiResponse(400, "نام کاربری یافت نشد"));

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded) return BadRequest(new ApiResponse(400, "نام کاربری یا کلمه عبور اشتباه است"));
            return Ok(new UserTokenDto
            {
                userName = user.UserName,
                Token = await _tokenService.CreateToken(user),
                PhotoUrl = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url,
                Gender = user.Gender,
                KnownAs = user.KnownAs
            });
        }


        [HttpGet("IsExistUserName/{userName}")]
        public async Task<bool> IsExistUserName(string userName)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == userName.ToLower());
        }
    }
}