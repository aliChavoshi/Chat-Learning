using API.Controllers;
using API.Data;
using API.Errors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [HttpGet("not-found")]
        public ActionResult GetNotFoundRequest()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound(new ApiResponse(404,"موردی یافت نشد"));
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult GetServerErrorRequest()
        {
            var thing = _context.Users.Find(-1); //null
            var thingToReturn = thing.ToString();
            return Ok();
        }

        [HttpGet("bad-request")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400,"خطایی رخ داده است"));
        }

        [HttpGet("bad-request/{id}")]
        public ActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }

    }
}