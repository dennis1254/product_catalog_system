using Microsoft.AspNetCore.Mvc;
using ProductCatalogSystem.Core.Interfaces;
using ProductCatalogSystem.Core.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace ProductCatalogSystem.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        [SwaggerOperation(Summary = "Register new user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Response<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad Request", typeof(CustomBadRequest))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterRequest request)
        {
            var user = await _authService.FindUserByEmailAsync(request.Email);
            if (user !=null)
            {
                return Ok(new Response<string>
                {
                    Success = false,
                    Message = "user already exist"
                });
            }
            var result = await _authService.RegisterUser(request);
           
            return Ok(result);
        }
       
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Authenticate user")]
        [SwaggerResponse(StatusCodes.Status200OK, "Request Successful", typeof(Response<string>))]
        [SwaggerResponse(StatusCodes.Status400BadRequest,"Bad Request", typeof(CustomBadRequest))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LoginUser([FromBody] LoginRequest  user)
        {
            var resp = await _authService.Login(user);
            return Ok(resp);    
                    
        }
       

    }
}
