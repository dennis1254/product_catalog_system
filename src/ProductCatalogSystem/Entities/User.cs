using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductCatalogSystem.Core.Entities
{
    public class User :IdentityUser
    {
        public User()
        {
            CreatedAt = DateTime.Now;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        public int UserTypeId { get; set; }
        public bool IsSocialAccount { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public DateTime? LastLoginTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
