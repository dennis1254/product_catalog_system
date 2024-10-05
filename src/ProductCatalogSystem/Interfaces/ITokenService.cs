using System.Security.Claims;

namespace ProductCatalogSystem.Core.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(List<Claim> claims);
    }
}
