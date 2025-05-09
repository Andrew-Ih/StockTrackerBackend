using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using static StockTracker.Helper.GetBestOrWorstPerformingDailyStocksHelper;
using StockTracker.Models;

namespace StockTracker
{
    public class GetWorstPerformingDailyStocks
    {
        private readonly ILogger<GetWorstPerformingDailyStocks> _logger;

        public GetWorstPerformingDailyStocks(ILogger<GetWorstPerformingDailyStocks> logger)
        {
            _logger = logger;
        }

        [Function("GetWorstPerformingDailyStocks")]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post", Route = "Test/WorstPerformingStocks")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            List<TopOrWorstGainers>? worstGainers = await GetTopBestOrWorstPerformingStocksAsync("top_losers");

            return new OkObjectResult(worstGainers);
        }
    }
}
