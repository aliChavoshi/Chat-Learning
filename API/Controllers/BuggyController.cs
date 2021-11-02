using API.Controllers;
using API.Data;
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
        public ActionResult GetNotFound()
        {
            var thing = _context.Users.Find(-1);
            if (thing == null) return NotFound();
            return Ok(thing);
        }

        [HttpGet("server-error")]
        public ActionResult GetServerError()
        {
            var thing = _context.Users.Find(-1); //null
            var thingToReturn = thing.ToString(); //null . tostring
            return Ok(thingToReturn);
        }

        [HttpGet("bad-request/{id}")]
        public AcceptedResult BadRequest(int id)
        {
            return Accepted();
        }
    }
}