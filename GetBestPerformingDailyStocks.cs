using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static StockTracker.Helper.GetStockPerformanceListHelper;
using StockTracker.Models;

namespace StockTracker
{
    public class GetBestPerformingDailyStocks
    {
        private readonly ILogger<GetBestPerformingDailyStocks> _logger;
        private static readonly string? TopGainersRootElement = Environment.GetEnvironmentVariable("TopGainersRootElement");
        
        public GetBestPerformingDailyStocks(ILogger<GetBestPerformingDailyStocks> logger)
        {
            _logger = logger;
        }

        [Function("GetBestPerformingDailyStocks")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "BestPerformingStocks")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            if (TopGainersRootElement != null)
            {
                List<StockPerformers>? topGainers = await GetStockPerformanceListAsync(TopGainersRootElement);
                return new OkObjectResult(topGainers);
            }

            //return new OkObjectResult("No top-performing stocks data available.");
            return new NoContentResult(); // Returns 204 if no data is available
        }
    }
}
