

using ProductCatalogSystem.Infrastructure.Models;

namespace ProductCatalogSystem.Infrastructure.Interfaces
{
    public interface ISwitchInterbankService
    {
        Task<(SwitchFundTransferResponse Data, bool Success)> FundTransfer(SwitchFundTransferRequest request, string baseURL);
        Task<(SwitchNameEnquiryResponse Data, bool Success)> NameEnquiry(SwitchNameEnquiryRequest request, string baseURL);
        Task<(SwitchStatusQueryResponse Data, bool Success)> TransQuery(SwitchStatusQueryRequest request, string baseURL);
        Task<(SessionIdResponse Data, bool Success)> GenerateSessionId(string baseURL);
    }
}
