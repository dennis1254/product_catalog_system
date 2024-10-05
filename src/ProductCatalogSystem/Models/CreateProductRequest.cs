using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogSystem.Core.Models
{
    public class CreateProductRequest
    {
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public List<CreateInventoryRequest>? Inventories { get; set; }
    }
    public class CreateInventoryRequest
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public string? Location { get; set; } // e.g., Warehouse location
    }
   
}
