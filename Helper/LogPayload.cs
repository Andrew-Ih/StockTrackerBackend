using Microsoft.Extensions.Logging;

namespace StockTracker.Helper
{
    public static class LogPayload
    {
        public static void LogPayloadToApplicationInsights(ILogger _logger, string requestBody)
        {
            var logData = new { Payload = requestBody};
            _logger.LogInformation("Payload Information: {@LogData}", logData);
        }
    }
}
