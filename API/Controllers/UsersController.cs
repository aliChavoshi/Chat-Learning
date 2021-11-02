using System.Collections.Generic;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using API.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<List<Users>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{id:int}")]
        // [Authorize]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null) return BadRequest(new ApiResponse(400, "کاربری یافت نشد"));
            return user;
        }
    }
}