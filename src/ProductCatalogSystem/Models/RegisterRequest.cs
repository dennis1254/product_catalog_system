using ProductCatalogSystem.Core.CustomAttributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogSystem.Core.Models
{
    public class RegisterRequest
    {
        [Required]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        [SwaggerSchema(Description = "user email address")]
        [SwaggerSchemaExample("example@gmail.com")]
        public string? Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        [StringLength(11)]
        [SwaggerSchema(Description = "user phone number")]
        [SwaggerSchemaExample("08100029911")]
        public string? PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Password")]
        [SwaggerSchema(Description = "password must contain a minimum of 8 characters, must contain at least 1 uppercase and lowercase letters")]
        [SwaggerSchemaExample("Test@2023")]
        public string? Password { get; set; }
        [Required]
        [Display(Name = "First Name")]
        [SwaggerSchemaExample("David")]
        public string? FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [SwaggerSchemaExample("Oboli")]
        public string? LastName { get; set; }

    }
}
