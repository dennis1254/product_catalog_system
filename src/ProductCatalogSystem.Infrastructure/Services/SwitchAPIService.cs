using ProductCatalogSystem.Infrastructure.Interfaces;
using ProductCatalogSystem.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Diagnostics;

namespace ProductCatalogSystem.Infrastructure.Services
{
    public class SwitchAPIService : ISwitchAPIService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        public SwitchAPIService(IConfiguration config)
        {
            _config = config;
            _logger = Log.ForContext<SwitchAPIService>(); 
        }
        public async Task<(List<SwitchResponse> Data, bool Success)> GetSwitches()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            List<SwitchResponse> response = new List<SwitchResponse>();
            bool success = false;
           

            string url = _config["URL_CONFIG:SwitchList"];
            // string subKey = _config["SubscriptionKeys:SwitchList"];
            var restRequest = new RestRequest();
            restRequest.Method = Method.Get;
            restRequest.AddHeader("Content-Type", "application/json");
          //  restRequest.AddHeader("Subscription-Key", subKey);

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
                        var responseObj = JsonConvert.DeserializeObject<List<SwitchResponse>>(resp.Content);
                        response = responseObj;
                        success = resp.IsSuccessful;
                    }
                    else
                    {
                        _logger.Error("{method}::unable to get switched - http response message {message}", nameof(GetSwitches),resp.ErrorMessage);
                    }

                                     
                }
            }
            catch (Exception ex)
            {
               
                _logger.Error(ex, "{method}:: unable to get switches ", nameof(GetSwitches));
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}::Retrieved {switches} switches in {Elapsed} ms", nameof(GetSwitches), response.Count, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
    }
}
