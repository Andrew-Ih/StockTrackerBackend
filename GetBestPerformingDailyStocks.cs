using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static StockTracker.Helper.GetBestOrWorstPerformingDailyStocksHelper;
using StockTracker.Models;

namespace StockTracker
{
    public class GetBestPerformingDailyStocks
    {
        private readonly ILogger<GetBestPerformingDailyStocks> _logger;

        public GetBestPerformingDailyStocks(ILogger<GetBestPerformingDailyStocks> logger)
        {
            _logger = logger;
        }

        [Function("GetBestPerformingDailyStocks")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "BestPerformingStocks")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            List<TopOrWorstGainers>? topGainers = await GetTopBestOrWorstPerformingStocksAsync("top_gainers");

            if (topGainers == null)
            {
                return new BadRequestObjectResult("No top-performing stocks data available.");
            }

            return new OkObjectResult(topGainers);
        }
    }
}
