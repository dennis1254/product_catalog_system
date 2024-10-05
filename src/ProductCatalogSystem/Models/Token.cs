using System;

namespace ProductCatalogSystem.Core.Models
{
	public class Tokens
	{
		public string Email { get; set; }
		public string AccessToken { get; set; }
		//public string RefreshToken { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }
}
