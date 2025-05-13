using System.Text.Json;
using StockTracker.Models;

namespace StockTracker.Helper
{
    public static class GetStockPerformanceHelper
    {
        private static readonly string? AlphavantageStockPerformanceDetailsApi = Environment.GetEnvironmentVariable("AlphavantageStockPerformanceDetailsApi");       

        public static async Task<List<StockPerformers>?> GetStockPerformanceAsync(string stockType)
        {
            using HttpClient client = new();
            string? apiUrl = AlphavantageStockPerformanceDetailsApi;
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            List<StockPerformers>? result = [];

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                result = ProcessStockPerformaceData(responseData, stockType);
            }
            return result;
        }

        public static List<StockPerformers>? ProcessStockPerformaceData(string responseData, string stockType)
        {
            const int NUMBER_OF_STOCKS = 10;
            List<StockPerformers> gainersList = [];
            try
            {
                var stockData = JsonDocument.Parse(responseData);
                if (ValidateStockDataExists(stockData.RootElement, stockType, out JsonElement gainersArray))
                {
                    gainersList = ExtractStockPerformers(gainersArray, NUMBER_OF_STOCKS);
                }
                return gainersList;
            }
            catch (Exception)
            {
                return gainersList;
            }
        }

        private static List<StockPerformers> ExtractStockPerformers(JsonElement gainersArray, int numberOfStocks)
        {
            List<StockPerformers> gainersList = [];

            for (int i = 0; i < numberOfStocks; i++)
            {
                var gainer = gainersArray[i];
                gainersList.Add(new StockPerformers
                {
                    Ticker = ExtractStockDetailsFromJson(gainer, "ticker", "Unknown"),
                    Price = ExtractStockDetailsFromJson(gainer, "price", 0m),
                    ChangeAmount = ExtractStockDetailsFromJson(gainer, "change_amount", 0m),
                    ChangePercentage = ExtractStockDetailsFromJson(gainer, "change_percentage", 0m),
                    Volume = ExtractStockDetailsFromJson(gainer, "volume", 0m)
                });
            }
            return gainersList;
        }

        public static bool ValidateStockDataExists(JsonElement rootElement, string propertyName, out JsonElement propertyValue)
        {
            return rootElement.TryGetProperty(propertyName, out propertyValue);
        }

        public static T ExtractStockDetailsFromJson<T>(JsonElement element, string propertyName, T defaultValue)
        {
            if (element.TryGetProperty(propertyName, out var property) && property.ValueKind == JsonValueKind.String)
            {
                try
                {
                    return (T)Convert.ChangeType(property.GetString()?.Replace("%", "") ?? "0", typeof(T));
                }
                catch
                {
                    return defaultValue;
                }
            }
            return defaultValue;
        }
    }
}
