using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace ProductCatalogSystem.API
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {

        private readonly IConfiguration _config;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration config) : base(options, logger, encoder, clock)
        {
            this._config = config;
        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var adminUsername = _config["AdminSettings:Username"];
            var adminPassword = _config["AdminSettings:Password"];
            var authorizationHeader = Request.Headers["Authorization"].ToString();
            if (authorizationHeader != null && authorizationHeader.StartsWith("basic", StringComparison.OrdinalIgnoreCase))
            {
                var token = authorizationHeader.Substring("Basic ".Length).Trim();
                var credentialsAsEncodedString = Encoding.UTF8.GetString(Convert.FromBase64String(token));
                var credentials = credentialsAsEncodedString.Split(':');
                var username = credentials[0];
                var password = credentials[1];
                if (username.Equals(adminUsername) && password.Equals(adminPassword))
                {
                    var claims = new[] { new Claim("name", credentials[0]), new Claim(ClaimTypes.Role, "Admin") };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var claimsPrincipal = new ClaimsPrincipal(identity);
                    return  Task.FromResult(AuthenticateResult.Success(new AuthenticationTicket(claimsPrincipal, Scheme.Name)));
                }
            }
            Response.StatusCode = 401;
            Response.Headers.Add("WWW-Authenticate", "accessbankplc.com");
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
          
        }
    }
}
