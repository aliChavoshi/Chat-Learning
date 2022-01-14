using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.extensions;
using API.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserLikesController : BaseApiController
    {
        private readonly IUserLikeRepository _userLikeRepository;
        private readonly IUserRepository _userRepository;

        public UserLikesController(IUserLikeRepository userLikeRepository, IUserRepository userRepository = null)
        {
            _userLikeRepository = userLikeRepository;
            _userRepository = userRepository;
        }

        [HttpPost("Add-Like")]
        public async Task<IActionResult> AddLike([FromQuery] string targetUserName)
        {
            var sourceUserId = User.GetUserId();
            var targetUser = await _userRepository.GetUserByUserName(targetUserName);
            if (targetUser == null) return NotFound("user not found");
            if (sourceUserId == targetUser.Id) return BadRequest("you cannot like yourSelf");

            var userLike = await _userLikeRepository.GetUserLike(sourceUserId, targetUser.Id);
            if (userLike != null) return BadRequest("you already liked this user");

            await _userLikeRepository.AddLike(sourceUserId, targetUser.Id);
            if (await _userLikeRepository.SaveAsync())
                return Ok();
            return BadRequest();
        }
    }
}