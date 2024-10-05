using ProductCatalogSystem.Infrastructure.Interfaces;
using ProductCatalogSystem.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Diagnostics;

namespace ProductCatalogSystem.Infrastructure.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        public EnquiryService(IConfiguration config)
        {
            _config = config;
            _logger = Log.ForContext<EnquiryService>(); 
        }
        public async Task<(Account AccountDetails, bool Success)> GetAccountDetails(string accountNumber, string appId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Account response = null;
            bool success = false;
            var request = new
            {
                AccountNumber = accountNumber,
                AppId = appId,
            };

            string url = _config["URL_CONFIG:AccountEnquiry"];
            string subKey = _config["SubscriptionKeys:AccountEnquiry"];
            var restRequest = new RestRequest();
            restRequest.Method = Method.Post;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Subscription-Key", subKey);
            restRequest.AddBody(request);

            try
            {
                RestClientOptions options = new RestClientOptions(url)
                {
                    RemoteCertificateValidationCallback = (source, certificate, chain, sslPolicyError) => true
                };
                using (var client = new RestClient(options))
                {
                    var resp = await client.ExecuteAsync(restRequest);
                   if (resp.IsSuccessful)
                    {
                        var responseObj = JsonConvert.DeserializeObject<APIResponse<Account>>(resp.Content);
                        response = responseObj.Data;
                        success = resp.IsSuccessful;
                    }                   
                }
            }
            catch (Exception ex)
            {
               
                _logger.Error(ex, "{method}:: unable to get details with {accountNumber} ", nameof(GetAccountDetails), accountNumber);
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}::Account enquiry with {accountNumber} and response {@response} in {Elapsed} ms", nameof(GetAccountDetails), accountNumber, response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
    }
}
