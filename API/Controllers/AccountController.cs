using System.Security.Cryptography;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.interfaces;
using API.Models;
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
        public async Task<ActionResult<UserTokenDto>> Register(RegisterDto model)
        {
            if (await IsExistUserName(model.userName))
                return BadRequest("نام کاربری موجود میباشد");

            using var hmac = new HMACSHA512();
            var user = new Users
            {
                UserName = model.userName,
                PasswordSalt = hmac.Key,
                PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password)),
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new UserTokenDto
            {
                userName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        /// <summary>
        /// Login a <see cref="LoginDto"/>
        /// </summary>
        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDto>> Login([FromBody] LoginDto model)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName.ToLower() == model.userName.ToLower());
            if (user == null) return BadRequest("نام کاربری یافت نشد");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return BadRequest("کلمه عبور اشتباه است");
            }
            return new UserTokenDto
            {
                userName = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }

        private async Task<bool> IsExistUserName(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName.ToLower() == userName.ToLower());
        }
    }
}