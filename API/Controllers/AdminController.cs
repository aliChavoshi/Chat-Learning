using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class AdminController : BaseApiController
    {
        public async Task<IActionResult> GetUsersWithRoles()
        {
            return Ok();
        }
    }
}