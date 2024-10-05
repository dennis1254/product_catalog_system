using ProductCatalogSystem.Core.Entities;
using ProductCatalogSystem.Core.Models;

namespace ProductCatalogSystem.Core.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<string>> RegisterUser(RegisterRequest request);
        Task<LoginResponse> Login(LoginRequest request);
        Task<User> FindUserByEmailAsync(string email);
    }
}
