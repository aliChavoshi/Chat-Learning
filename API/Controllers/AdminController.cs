using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
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
        private readonly DataContext _context;

        public AdminController(UserManager<Users> userManager, DataContext context)
        {
            _userManager = userManager;
            _context = context;
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
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null) return NotFound(new ApiResponse(400, userName + "یافت نشد"));
            //delete old roles
            var userRole = _context.UserRoles.Where(x => x.UserId == user.Id).ToList();
            if (userRole.Any() && userRole.Count > 0)
            {
                _context.UserRoles.RemoveRange(userRole);
                await _context.SaveChangesAsync();
            }
            //add selected roles to user
            if (selectedRoles.Any() && selectedRoles.Count() > 0)
            {
                var addRes = await _userManager.AddToRolesAsync(user, selectedRoles);
                if (!addRes.Succeeded) return BadRequest(new ApiResponse(400, "خطا در ثبت اطلاعات"));
                return Ok(selectedRoles);
            }
            return null;
        }
    }
}