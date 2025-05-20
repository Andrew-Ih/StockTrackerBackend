using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static StockTracker.Helper.GetStockDetailsHelper;
using StockTracker.Models;

namespace StockTracker
{
    public class GetStockDetails
    {
        private readonly ILogger<GetStockDetails> _logger;

        public GetStockDetails(ILogger<GetStockDetails> logger)
        {
            _logger = logger;
        }

        [Function("GetStockDetails")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "StockDetails")] HttpRequest req, ILogger log)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            // Log the request payload to Application Insights
            _logger.LogInformation("Received Payload: {RequestBody}", requestBody);

            StockDetails? stockObject = await GetStockObjectDetailsAsync(requestBody);

            if (stockObject?.IsNull == true)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(stockObject);
        }
    }
}
