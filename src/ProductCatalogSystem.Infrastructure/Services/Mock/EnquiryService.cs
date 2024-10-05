using ProductCatalogSystem.Infrastructure.Interfaces;
using ProductCatalogSystem.Infrastructure.Models;

namespace ProductCatalogSystem.Infrastructure.Services.Mock
{
    public class EnquiryService : IEnquiryService
    {
        public async Task<(Account AccountDetails, bool Success)> GetAccountDetails(string accountNumber, string appId)
        {
            return (new Account()
            {
                AccountNumber = accountNumber,
                AccountName = "Chinemelum Agbapulonwu",
                CurrencyCode = "NGN",
                AccountType = "SAVINGS",
                BVN = "22236190142",
                AvailableBalance = 6000000
            }, true);
        }
    }
}
