using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using StockTracker.Models;
using static StockTracker.Helper.GetStockPerformanceHelper;

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
                List<StockPerformers>? topGainers = await GetStockPerformanceAsync(TopGainersRootElement);
                if (topGainers?.Count != 0)
                {
                    return new OkObjectResult(topGainers);
                }           
            }
;
            return new NoContentResult(); 
        }
    }
}
