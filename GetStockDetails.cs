using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static StockTracker.Helper.GetStockDetailsHelper;
using StockTracker.Models;
using System.Text.Json;

namespace StockTracker
{
    public class GetStockDetails
    {
        private readonly ILogger<GetStockDetails> _logger;

        public GetStockDetails(ILogger<GetStockDetails> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Test/StockDetails")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            Stock? stockObject = await GetStockObjectDetailsAsync(requestBody);

            return new OkObjectResult(stockObject);
        }
    }
}
