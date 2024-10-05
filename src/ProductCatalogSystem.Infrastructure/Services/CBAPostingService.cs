using ProductCatalogSystem.Infrastructure.Interfaces;
using ProductCatalogSystem.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Diagnostics;

namespace ProductCatalogSystem.Infrastructure.Services
{
    public class CBAPostingService : ICBAPosting
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        public CBAPostingService(IConfiguration config)
        {
            _config = config;
            _logger = Log.ForContext<CBAPostingService>();
        }
        public async Task<(SinglePostResponse Data,bool Success)> SinglePost(SinglePostRequest request)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SinglePostResponse response = null;
            bool success = false;
            string url = _config["URL_CONFIG:SinglePost"];
            string subKey = _config["SubscriptionKeys:PostingService"];
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
                        response = JsonConvert.DeserializeObject<SinglePostResponse>(resp.Content);
                    }
                    success = resp.IsSuccessful;
                    _logger.Debug("{method}::api response {@response}", nameof(SinglePost), resp);

                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}:: unable to post transaction with transaction reference {transactionReference} and request object {@request}", nameof(SinglePost), request.Reference, request);
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}::Transaction posted with transaction reference {transactionReference} and response {@response} in {Elapsed} ms", nameof(SinglePost), request.Reference,response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
        public async Task<(BatchPostResponse Data, bool Success)> BatchPost(BatchPostRequest request)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            BatchPostResponse response = null;
            bool success = false;
            string url = _config["URL_CONFIG:BatchPost"];
            string subKey = _config["SubscriptionKeys:PostingService"];
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
                        response = JsonConvert.DeserializeObject<BatchPostResponse>(resp.Content);
                    }
                    success = resp.IsSuccessful;
                    _logger.Debug("{method}::api response {@response}", nameof(BatchPost), resp);

                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}:: unable to post transaction with transaction reference {transactionReference} and request object {@request}", nameof(BatchPost), request.Reference, request);
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}::Transaction posted with transaction reference {transactionReference} and response {@response} in {Elapsed} ms", nameof(BatchPost), request.Reference, response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
    }
}
