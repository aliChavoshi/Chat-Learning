using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        private readonly UserManager<Users> _userManager;

        public AdminController(UserManager<Users> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("GetUsersWithRoles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _userManager.Users.Include(x => x.UserRole).ThenInclude(x => x.Role).OrderBy(x => x.UserName)
                .Select(x => new
                {
                    id = x.Id,
                    userName = x.UserName,
                    roles = x.UserRole.Select(y => y.Role.Name).ToList(),
                    roleIds = x.UserRole.Select(r => r.RoleId).ToList()
                }).ToListAsync();
            return Ok(users);
        }

        [HttpPost("edit-role/{userName}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> EditRole(string userName, [FromQuery] string roles) // admin,member => split(,) => [admin,member]
        {
            var selectedRoles = roles.Split(","); //new role -> user
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) return NotFound(new ApiResponse(400, userName + "یافت نشد"));
            var userRoles = await _userManager.GetRolesAsync(user); //database
            //delete old roles
            var removeRes = await _userManager.RemoveFromRolesAsync(user, userRoles);
            if (!removeRes.Succeeded) return BadRequest(new ApiResponse(400, "خطا در حذف اطلاعات"));
            //add selected roles to user
            var addRes = await _userManager.AddToRolesAsync(user, selectedRoles);
            if (!addRes.Succeeded) return BadRequest(new ApiResponse(400, "خطا در ثبت اطلاعات"));
            return Ok(selectedRoles);
        }
    }
}