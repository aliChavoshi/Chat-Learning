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

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(ITokenService tokenService, DataContext context)
        {
            _tokenService = tokenService;
            _context = context;
        }

        /// <summary>
        /// Register a new  <see cref="RegisterDto"/>
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserTokenDto>> Register(RegisterDto model)
        {
            if (await IsExistUserName(model.userName))
                return BadRequest(new ApiResponse(400, model.userName + "یافت نشد"));

            using var hmac = new HMACSHA512();
            var user = new Users
            {
                UserName = model.userName,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password)),
            };
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Ok(new UserTokenDto
                {
                    userName = user.UserName,
                    Token = _tokenService.CreateToken(user)
                });
            }
            return BadRequest(new ApiResponse(400, "خطا در ثبت اطلاعات"));
        }

        /// <summary>
        /// Login a <see cref="LoginDto"/>
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginDto model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == model.userName.ToLower());
            if (user == null) return BadRequest(new ApiResponse(400, "نام کاربری یافت نشد"));

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return BadRequest(new ApiResponse(400, "رمز عبور اشتباه است"));
            }
            return Ok(new UserTokenDto
            {
                userName = user.UserName,
                Token = _tokenService.CreateToken(user)
            });
        }

        private async Task<bool> IsExistUserName(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}