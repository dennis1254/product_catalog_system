using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ProductCatalogSystem.API.Controllers
{
    [ApiController]
   // [Authorize]
    public class SecureAPIController : ControllerBase
    {
        public readonly int _userId;
        public readonly string _fullName;
        public SecureAPIController(IHttpContextAccessor httpContext)
        {
            var claims = httpContext.HttpContext.User.Claims.ToList();
            _userId = Convert.ToInt32(claims?.FirstOrDefault(x => x.Type.Equals("UserId", StringComparison.OrdinalIgnoreCase))?.Value);
            _fullName = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;
        }
    }
}
