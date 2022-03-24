using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Errors;
using API.extensions;
using API.Helpers;
using API.interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserLikesController : BaseApiController
    {
        private readonly IUnitOfWork _uow;
        public UserLikesController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpPost("Add-Like")]
        // [Authorize(Roles = "member")]
        public async Task<IActionResult> AddLike([FromQuery] string targetUserName)
        {
            var sourceUserId = User.GetUserId();
            var targetUser = await _uow.UserRepository.GetUserByUserName(targetUserName);
            if (targetUser == null) return NotFound("user not found");
            if (sourceUserId == targetUser.Id) return BadRequest("you cannot like yourSelf");

            var userLike = await _uow.UserLikeRepository.GetUserLike(sourceUserId, targetUser.Id);
            if (userLike != null) return BadRequest(new ApiResponse(400, "you already liked this user"));

            await _uow.UserLikeRepository.AddLike(sourceUserId, targetUser.Id);
            if (await _uow.CompleteAsync())
                return Ok();
            return BadRequest();
        }

        [HttpGet("get-likes")]
        public async Task<ActionResult<PagedList<MemberDto>>> GetUserLikes([FromQuery] GetLikeParams getLikeParams)
        {
            return Ok(await _uow.UserLikeRepository.GetUserLikes(getLikeParams, User.GetUserId()));
        }
    }
}