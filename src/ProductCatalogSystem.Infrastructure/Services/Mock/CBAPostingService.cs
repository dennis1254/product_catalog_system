using ProductCatalogSystem.Infrastructure.Interfaces;
using ProductCatalogSystem.Infrastructure.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System.Diagnostics;

namespace ProductCatalogSystem.Infrastructure.Services.Mock
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

        public async Task<(BatchPostResponse Data, bool Success)> BatchPost(BatchPostRequest request)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            BatchPostResponse response = null;
            bool success = false;
            var status = _config["MockingResponseSettings:SinglePost"];

            switch (status.ToLower())
            {
                case "success":
                    response = new BatchPostResponse
                    {
                        TranId = $"0199NIP{DateTime.Now.Ticks.ToString("000000000")}y",
                        Status = status,
                        Message = "Approved and successfully posted",
                        PostedEntries = request.Entries.Count
                    };
                    success = true;
                    break;
                case "fail":
                    response = new BatchPostResponse
                    {
                        TranId = "",
                        Status = status,
                        Message = status,                      
                    };
                    break;
                case "error":

                    _logger.Error(new Exception("Mock exception simulated"), "{method}:: unable to post transaction with transaction reference {transactionReference} and request object {@request}", nameof(SinglePost), request.Reference, request);
                    break;
                default:
                    break;
            }

            stopwatch.Stop();

            _logger.Information("{method}::Transaction posted with transaction reference {transactionReference} and response {@response} in {Elapsed} ms", nameof(SinglePost), request.Reference, response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }

        public async Task<(SinglePostResponse Data,bool Success)> SinglePost(SinglePostRequest request)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            SinglePostResponse response = null;
            bool success = false;
            var status = _config["MockingResponseSettings:SinglePost"];

            switch (status.ToLower())
            {
                case "success":
                    response = new SinglePostResponse
                    {
                        TranId = $"0199NIP{DateTime.Now.Ticks.ToString("000000000")}y",
                        Status = status,
                        Message = "Approved and successfully posted"
                    };
                    success = true;
                    break;
                case "fail":
                    response = new SinglePostResponse
                    {
                        TranId = "",
                        Status = status,
                        Message = status
                    };
                    break;
                case "error":
                    
                    _logger.Error(new Exception("Mock exception simulated"), "{method}:: unable to post transaction with transaction reference {transactionReference} and request object {@request}", nameof(SinglePost), request.Reference, request);
                    break;
                default:
                    break;
            }
                
             stopwatch.Stop();
            
            _logger.Information("{method}::Transaction posted with transaction reference {transactionReference} and response {@response} in {Elapsed} ms", nameof(SinglePost), request.Reference,response, stopwatch.ElapsedMilliseconds);

            return (response, success);
        }
    }
}
