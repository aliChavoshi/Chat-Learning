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

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public AccountController(ITokenService tokenService, IAccountRepository accountRepository, IMapper mapper)
        {
            _tokenService = tokenService;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Register a new  <see cref="RegisterDto"/>
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserTokenDto>> Register(RegisterDto model)
        {
            if (await _accountRepository.IsExistUserName(model.UserName))
                return BadRequest(new ApiResponse(400, model.UserName + "یافت نشد"));

            using var hmac = new HMACSHA512();
            var userEntity = _mapper.Map<Users>(model);
            userEntity.PasswordSalt = hmac.Key;
            userEntity.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));
            await _accountRepository.AddUser(userEntity);
            if (await _accountRepository.SaveChangeAsync())
            {
                return Ok(new UserTokenDto
                {
                    userName = userEntity.UserName,
                    Token = _tokenService.CreateToken(userEntity)
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
            var user = await _accountRepository.GetUserByUserNameWithPhotos(model.userName);
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
                Token = _tokenService.CreateToken(user),
                PhotoUrl = user?.Photos?.FirstOrDefault(x => x.IsMain)?.Url
            });
        }


        [HttpGet("IsExistUserName/{userName}")]
        public async Task<ActionResult<bool>> IsExistUserName(string userName)
        {
            return await _accountRepository.IsExistUserName(userName);
        }
    }
}