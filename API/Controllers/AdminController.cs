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
                .Select(x => new UserTokenDto
                {
                    userName = x.UserName,
                    Roles = x.UserRole.Select(r => r.Role.Name).ToList(),
                    Gender = x.Gender,
                    KnownAs = x.KnownAs,
                    Id = x.Id
                }).ToListAsync();
            return Ok(users);
        }

        [HttpPost("edit-role/{userName}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> EditRole(string userName, [FromQuery] string roles) // admin,member => split(,) => [admin,member]
        {
            var selectedRoles = roles.Split(","); //new role -> user
            var user = await _userManager.Users.Include(x => x.UserRole).ThenInclude(x => x.Role).FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null) return NotFound(new ApiResponse(400, userName + "یافت نشد"));
            var userRoles = user.UserRole.ToList(); //database
            var resRoles = new List<string>();
            //delete old roles
            foreach (var role in selectedRoles)
            {
                if (userRoles.Select(x => x.Role.Name).Contains(role))
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                }
                else
                {
                    resRoles.Add(role);
                }
            }
            var deleteRoles = userRoles.Where(x => !selectedRoles.Contains(x.Role.Name)).ToList();
            foreach (var role in deleteRoles)
            {
                await _userManager.RemoveFromRoleAsync(user, role.Role.Name);
            }
            // var removeRes = await _userManager.RemoveFromRolesAsync(user, userRoles.Select(x => x.Role.Name).ToArray());
            // if (!removeRes.Succeeded) return BadRequest(new ApiResponse(400, "خطا در حذف اطلاعات"));
            //add selected roles to user
            var addRes = await _userManager.AddToRolesAsync(user, resRoles);
            if (!addRes.Succeeded) return BadRequest(new ApiResponse(400, "خطا در ثبت اطلاعات"));
            return Ok(selectedRoles);
        }
    }
}