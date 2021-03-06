using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Errors;
using API.interfaces;
using API.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API.extensions;
using Microsoft.AspNetCore.Http;
using API.Entities;
using API.Helpers;
using API.Middlewares;

namespace API.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(LogUserActivity))]
    public class UsersController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        private readonly IPhotoService _photoService;
        private readonly IMapper _mapper;
        public UsersController(IUnitOfWork unitOfWork, IPhotoService photoService, IMapper mapper)
        {
            this._uow = unitOfWork;
            _photoService = photoService;
            _mapper = mapper;
        }


        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            userParams.currentUserName = User.GetUserName();
            var users = await _uow.UserRepository.GetAllUsersMemberDto(userParams);
            // Response.AddPaginationHeader(users.CurrentPage, itemsPerPage: users.PageSize, totalItems: users.TotalCount, totalPages: users.TotalPage);
            return Ok(users);
        }

        [HttpGet("getUserById/{id:int}")]
        public async Task<ActionResult<MemberDto>> GetUserById(int id)
        {
            var user = await _uow.UserRepository.GetMemberDtoById(id);
            if (user == null) return NotFound(new ApiResponse(404, "چنین کاربری یافت نشد"));
            return Ok(user);
        }

        [HttpGet("getUserByUserName/{userName}", Name = "GetUser")]
        public async Task<ActionResult<MemberDto>> GetUserByUserName(string userName)
        {
            var user = await _uow.UserRepository.GetMemberDtoByUserName(userName);
            if (user == null) return NotFound(new ApiResponse(404, "چنین کاربری یافت نشد"));
            return Ok(user);
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult<MemberDto>> UpdateUser([FromBody] MemberUpdateDto memberDto)
        {
            var username = User?.GetUserName();
            var member = await _uow.UserRepository.GetUserByUserNameWithPhotos(username);
            if (member == null) return NotFound(new ApiResponse(404));

            member = _mapper.Map(memberDto, member);
            _uow.UserRepository.Update(member);
            if (await _uow.CompleteAsync())
                return Ok(_mapper.Map<MemberDto>(member));

            return BadRequest(new ApiResponse(400));
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(new ApiResponse(400, "عملیات با شکست روبرو شد"));

            var user = await _uow.UserRepository.GetUserByUserNameWithPhotos(User.GetUserName());
            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId,
                UserId = user.Id,
                IsMain = user.Photos.Count == 0 ? true : false
            };
            user.Photos.Add(photo);
            _uow.UserRepository.Update(user);
            if (await _uow.CompleteAsync())
                return CreatedAtRoute("GetUser", new { userName = user.UserName }, _mapper.Map<PhotoDto>(photo));
            return BadRequest(new ApiResponse(400, "عملیات با شکست روبرو شد"));
        }

        [HttpPut("SetMainPhoto/{photoId}")]
        public async Task<ActionResult<PhotoDto>> SetMainPhoto(int photoId)
        {
            var user = await _uow.UserRepository.GetUserByUserNameWithPhotos(HttpContext.User.GetUserName());
            if (user == null) return NotFound(new ApiResponse(404, "کاربری یافت نشد"));
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound(new ApiResponse(404, "تصویری یافت نشد"));
            if (photo.IsMain) return BadRequest(new ApiResponse(400, "این تصویر به عنوان تصویر پیش فرض میباشد "));

            var mainPhoto = user.Photos.FirstOrDefault(x => x.IsMain);
            mainPhoto.IsMain = false;
            photo.IsMain = true;
            _uow.UserRepository.Update(user);
            if (await _uow.CompleteAsync()) return Ok(_mapper.Map<PhotoDto>(photo));
            return BadRequest(new ApiResponse(400));
        }


        [HttpDelete("DeletePhoto/{photoId}")]
        public async Task<IActionResult> DeletePhoto(int photoId)
        {
            var user = await _uow.UserRepository.GetUserByUserNameWithPhotos(User.GetUserName());
            if (user == null) return NotFound(new ApiResponse(404));
            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
            if (photo == null) return NotFound(new ApiResponse(404));
            if (photo.IsMain) return BadRequest(new ApiResponse(400, "شما نمیتوانید عکس پیش فرض را پاک کنید"));
            var result = await _photoService.DeletePhotoAsync(photo.PublicId);
            //TODO : Check image for Delete from cloudinary
            user.Photos.Remove(photo);
            _uow.UserRepository.Update(user);
            if (await _uow.CompleteAsync()) return Ok(_mapper.Map<PhotoDto>(photo));
            return BadRequest(new ApiResponse(400));
        }
    }
}