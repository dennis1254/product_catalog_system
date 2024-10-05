namespace ProductCatalogSystem.Core.Models
{
    public class LoginResponse:Response<Tokens>
    {
        public bool EmailVerified { get; set; }
    }
}
