using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using StockTracker.Models;
using static StockTracker.Helper.GetStockPerformanceHelper;

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
                List<StockPerformers>? worstGainers = await GetStockPerformanceAsync(TopLosersRootElement);
                if (worstGainers?.Count != 0)
                {
                    return new OkObjectResult(worstGainers);
                }           
            }

            return new NoContentResult(); 
        }
    }
}
