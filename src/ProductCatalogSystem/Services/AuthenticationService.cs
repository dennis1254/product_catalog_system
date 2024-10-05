
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using ProductCatalogSystem.Core.Entities;
using ProductCatalogSystem.Core.Interfaces;
using ProductCatalogSystem.Core.Models;
using ProductCatalogSystem.Repositories;
using Serilog;
using System.Security.Claims;
using ILogger = Serilog.ILogger;

namespace ProductCatalogSystem.Core.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IMapper _mapper;       
        private readonly UserManager<User> _userManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;
        public AuthenticationService(IMapper mapper, IConfiguration configuration, UserManager<User> userManager, IUnitOfWork unitOfWork, ITokenService tokenService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _userManager = userManager;
            this._logger = Log.ForContext<AuthenticationService>();
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
        }

        public async Task<Response<string>> RegisterUser(RegisterRequest registerRequest)
        {
            try
            {
                var user = _mapper.Map<User>(registerRequest);
                var exist = await _userManager.FindByEmailAsync(user.UserName);
                if (exist != null)
                {
                    return new Response<string>
                    {
                        Success = false,
                        Message = "email address already taken"
                    };
                }
                var resp = await _userManager.CreateAsync(user, registerRequest.Password);
                string errorMessage = string.Empty;
                if (!resp.Succeeded)
                {
                    foreach (var error in resp.Errors)
                    {
                        errorMessage =string.IsNullOrEmpty(errorMessage)?error.Description: errorMessage + ", " + error.Description;
                    }
                }
                return new Response<string>
                {
                    Success = resp.Succeeded,
                    Message = resp.Succeeded ? "Registration successful" : errorMessage,
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex,"Failed to register user with email {email}",registerRequest.Email);
                return new Response<string>
                {
                    Success =false,
                    Message = "Something went wrong",                    
                };
            }
        }
        public async Task<(bool Success,User User)> ValidateUser(LoginRequest userLogin)
        {
           var user = await _userManager.FindByEmailAsync(userLogin.Email); 
            var result = (user != null && await _userManager.CheckPasswordAsync(user, userLogin.Password));
            return (result,user);
        }
       
        public async Task<User> FindUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;
        }

       
        private List<Claim> GetClaims(string email)
        {
            var claims = new List<Claim>{new Claim(ClaimTypes.Email, email)};            
            return claims;
        }
        private List<Claim> GetClaims(User user)
        {
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, string.Format("{0} {1}",user.FirstName,user.LastName)),
                new Claim("UserId",user.UserId.ToString()),
                new Claim("PhoneNumber",string.IsNullOrWhiteSpace(user.PhoneNumber)?"":user.PhoneNumber)
            };
            return claims;
        }

        public async Task<LoginResponse> Login(LoginRequest loginDto)
        {
            var IsValid = await ValidateUser(loginDto);
            if (IsValid.Success)
            {
                var claims = GetClaims(IsValid.User);
                var token = _tokenService.GenerateToken(claims);
                return new LoginResponse
                {
                    Success = true,
                    Message = "Successful",
                    EmailVerified = true,
                    Data = new Tokens
                    {
                        Email = loginDto.Email,
                        AccessToken = token,
                        ExpiresAt = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryTime"]))
                    }
                };
            }
            else
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Invalid login credentials"
                };
            }
        }
        
    }
}
