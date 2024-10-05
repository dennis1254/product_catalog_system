using ProductCatalogSystem.Infrastructure.Interfaces;
using ProductCatalogSystem.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Diagnostics;

namespace ProductCatalogSystem.Infrastructure.Services
{
    public class ChannelAPIService : IChannelAPIService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        public ChannelAPIService(IConfiguration config)
        {
            _config = config;
            _logger = Log.ForContext<ChannelAPIService>(); 
        }
        public async Task<(List<ChannelResponse> Data, bool Success)> GetChannels()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            List<ChannelResponse> response = new List<ChannelResponse>();
            bool success = false;
           

            string url = _config["URL_CONFIG:ChannelList"];
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
                        var responseObj = JsonConvert.DeserializeObject<List<ChannelResponse>>(resp.Content);
                        response = responseObj;
                        success = resp.IsSuccessful;
                    }
                    else
                    {
                        _logger.Error("{method}::unable to get channels - http response message {message}", nameof(GetChannels),resp.ErrorMessage);
                    }

                                     
                }
            }
            catch (Exception ex)
            {
               
                _logger.Error(ex, "{method}:: unable to get channels ", nameof(GetChannels));
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}::Retrieved {channels} channels in {Elapsed} ms", nameof(GetChannels), response.Count, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
    }
}
