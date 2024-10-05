using ProductCatalogSystem.Infrastructure.Interfaces;
using ProductCatalogSystem.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ProductCatalogSystem.Infrastructure.Services
{
    public class SwitchInterbankService : ISwitchInterbankService
    {
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly bool _useDefaultBaseUrl;
        public SwitchInterbankService(IConfiguration config)
        {
            _config = config;
            _logger = Log.ForContext<SwitchInterbankService>();
            _useDefaultBaseUrl = Convert.ToBoolean(config["SwitchConfig:UseDefaultBaseURL"]);
        }

        public async Task<(SwitchFundTransferResponse Data, bool Success)> FundTransfer(SwitchFundTransferRequest request, string baseURL)
        {
            baseURL = Regex.Replace(baseURL, @"\s+", string.Empty);
            if (_useDefaultBaseUrl)
            {
                baseURL = baseURL.Replace("https://ng-dev.default/", "http://api.dev.accessbankplc.com/");
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SwitchFundTransferResponse response = null;
            bool success = false;

            string path = _config["URL_CONFIG:FundTransfer"];
            string url = baseURL + path;
            string subKey = _config["SubscriptionKeys:SwitchInterbank"];
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
                        var responseObj = JsonConvert.DeserializeObject<SwitchFundTransferResponse>(resp.Content);
                        response = responseObj;
                        success = resp.IsSuccessful;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}:: an error occurred during FT execution with payment reference {paymentReference} {@request} ", nameof(FundTransfer),request.PaymentReference, request);
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}::FT with payment reference {paymentReference} and response {@response} in {Elapsed} ms", nameof(FundTransfer), request.PaymentReference, response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }

      
        public async Task<(SwitchNameEnquiryResponse Data, bool Success)> NameEnquiry(SwitchNameEnquiryRequest request, string baseURL)
        {
            baseURL = Regex.Replace(baseURL, @"\s+", string.Empty);
            if(_useDefaultBaseUrl)
            {
                baseURL = baseURL.Replace("https://ng-dev.default/", "http://api.dev.accessbankplc.com/");
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SwitchNameEnquiryResponse response = null;
            bool success = false;

            string path = _config["URL_CONFIG:NameEnquiry"];
            string url = baseURL + path;
            string subKey = _config["SubscriptionKeys:SwitchInterbank"];
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
                        var responseObj = JsonConvert.DeserializeObject<SwitchNameEnquiryResponse>(resp.Content);
                        response = responseObj;
                        success = resp.IsSuccessful;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}:: an error occurred during name enquiry with account number {accountNumber} {@request} ", nameof(NameEnquiry), request.RecipientAccountNumber, request);
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}::Name enquiry with account number {accountNumber} and response {@response} in {Elapsed} ms", nameof(NameEnquiry), request.RecipientAccountNumber, response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }

        public async Task<(SwitchStatusQueryResponse Data, bool Success)> TransQuery(SwitchStatusQueryRequest request, string baseURL)
        {
            baseURL = Regex.Replace(baseURL, @"\s+", string.Empty);
            if (_useDefaultBaseUrl)
            {
                baseURL = baseURL.Replace("https://ng-dev.default/", "http://api.dev.accessbankplc.com/");
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SwitchStatusQueryResponse response = null;
            bool success = false;

            string path = _config["URL_CONFIG:StatusQuery"];
            string url = baseURL + path;
            string subKey = _config["SubscriptionKeys:SwitchInterbank"];
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
                        var responseObj = JsonConvert.DeserializeObject<SwitchStatusQueryResponse>(resp.Content);
                        response = responseObj;
                        success = resp.IsSuccessful;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}:: an error occurred during Status query with sessionId {sessionId} {@request} ", nameof(TransQuery), request.SessionId, request);
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}:: Status query with sessionId {sessionId} and response {@response} in {Elapsed} ms", nameof(FundTransfer), request.SessionId, response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
        public async Task<(SessionIdResponse Data, bool Success)> GenerateSessionId(string baseURL)
        {
            baseURL = Regex.Replace(baseURL, @"\s+", string.Empty);
            if (_useDefaultBaseUrl)
            {
                baseURL = baseURL.Replace("https://ng-dev.default/", "http://api.dev.accessbankplc.com/");
            }
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SessionIdResponse response = null;
            bool success = false;

            string path = _config["URL_CONFIG:GenerateSessionId"];
            string url = baseURL + path;
            string subKey = _config["SubscriptionKeys:SwitchInterbank"];
            var restRequest = new RestRequest();
            restRequest.Method = Method.Get;
            restRequest.AddHeader("Content-Type", "application/json");
            restRequest.AddHeader("Subscription-Key", subKey);

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
                        var responseObj = JsonConvert.DeserializeObject<SessionIdResponse>(resp.Content);
                        response = responseObj;
                        success = resp.IsSuccessful;
                    }
                }
            }
            catch (Exception ex)
            {

                _logger.Error(ex, "{method}::unable to generate session id", nameof(GenerateSessionId));
            }
            finally
            {
                stopwatch.Stop();
            }

            _logger.Information("{method}:: Generate session id response {@response} in {Elapsed} ms", nameof(GenerateSessionId), response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
    }
}
