using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [Authorize]
        [HttpGet("/haha")]
        public IActionResult TestGet()
        {
            return Ok("Haha");
        }
    }
}
