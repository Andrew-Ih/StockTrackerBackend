using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static StockTracker.Helper.GetStockPerformanceListHelper;
using StockTracker.Models;

namespace StockTracker
{
    public class GetWorstPerformingDailyStocks
    {
        private readonly ILogger<GetWorstPerformingDailyStocks> _logger;
        private static readonly string? TopLosersRootElement = Environment.GetEnvironmentVariable("TopLosersRootElement");
        
        public GetWorstPerformingDailyStocks(ILogger<GetWorstPerformingDailyStocks> logger)
        {
            _logger = logger;
        }

        [Function("GetWorstPerformingDailyStocks")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "WorstPerformingStocks")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (TopLosersRootElement != null)
            {
                List<StockPerformers>? worstGainers = await GetStockPerformanceListAsync(TopLosersRootElement);
                return new OkObjectResult(worstGainers);
            }

            //return new OkObjectResult("No worst-performing stocks data available."); 
            return new NoContentResult(); // Returns 204 if no data is available
        }
    }
}
