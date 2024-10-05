using ProductCatalogSystem.Core.CustomAttributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalogSystem.Core.Models
{
    public class UpdateProductRequest
    {
        [Required]
        [MaxLength(100)]
        [SwaggerSchema(Description = "Product name")]
        [SwaggerSchemaExample("Samsung S24")]
        public string? Name { get; set; }

        [Required]
        [MaxLength(500)]
        [SwaggerSchema(Description = "Product description")]
        [SwaggerSchemaExample("The Samsung Galaxy S24 comes with 6.1-nch Dynamic AMOLED display with 120Hz refresh rate and Qualcomm Snapdragon 8 Gen 3 processor")]
        public string? Description { get; set; }
    }
   
}
