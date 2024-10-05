using ProductCatalogSystem.Core.CustomAttributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogSystem.Core.Models
{
    public class LoginRequest
    {
        [Required]
        [Display(Name = "Email Address")]
        [SwaggerSchema(Description = "user email address")]
        [SwaggerSchemaExample("example@gmail.com")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Password")]
        [SwaggerSchema(Description = "password must contain a minimum of 8 characters, must contain at least 1 uppercase and lowercase letters")]
        [SwaggerSchemaExample("Test@2023")]
        public string Password { get; set; }

    }

   
}
