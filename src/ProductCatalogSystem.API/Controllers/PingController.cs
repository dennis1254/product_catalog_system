using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Interbank.API.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class PingController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {            
            return Ok();
        }


    }
}
