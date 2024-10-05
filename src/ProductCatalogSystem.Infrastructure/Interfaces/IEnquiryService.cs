using ProductCatalogSystem.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalogSystem.Infrastructure.Interfaces
{
    public interface IEnquiryService
    {
        Task<(Account AccountDetails, bool Success)> GetAccountDetails(string accountNumber, string appId);
    }
}
