using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ProductCatalogSystem.Entities
{
    public class Inventory
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public string? Location { get; set; } // e.g., Warehouse location

        // Foreign key to the Product entity
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        // Navigation property to the related Product entity
        [JsonIgnore]
        public Product? Product { get; set; }

    }
}
