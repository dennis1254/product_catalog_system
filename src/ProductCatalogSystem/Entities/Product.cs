
using ProductCatalogSystem.Core.CustomAttributes;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogSystem.Entities
{
    public class Product
    {
        public Product()
        {
            CreatedAt = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        [SwaggerSchema(Description = "Product name")]
        [SwaggerSchemaExample("Samsung S24")]
        public string? Name { get; set; }

        [MaxLength(500)]
        [SwaggerSchema(Description = "Product description")]
        [SwaggerSchemaExample("The Samsung Galaxy S24 comes with 6.1-nch Dynamic AMOLED display with 120Hz refresh rate and Qualcomm Snapdragon 8 Gen 3 processor")]

        public string? Description { get; set; }

        public ICollection<Inventory> Inventories { get; set; }         // Navigation property for related Inventory entities
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }

    }
}
