using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StockTracker.Models;

namespace StockTracker.Helper
{
    public static class GetStockPerformanceListHelper
    {
        private static readonly string? AlphavantageStockPerformanceDetailsApi = Environment.GetEnvironmentVariable("AlphavantageStockPerformanceDetailsApi");       

        public static async Task<List<StockPerformers>?> GetStockPerformanceListAsync(string stockType)
        {
            using HttpClient client = new();
            string? apiUrl = AlphavantageStockPerformanceDetailsApi;
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            List<StockPerformers>? result = null;

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                result = ProcessStockPerformaceData(responseData, stockType);
            }
            return result;
        }

        public static List<StockPerformers>? ProcessStockPerformaceData(string responseData, string stockType)
        {
            try
            {
                var stockData = JsonDocument.Parse(responseData);

                // Ensure "top_gainers or top_losers" property exists in JSON response
                if (!stockData.RootElement.TryGetProperty(stockType, out JsonElement gainersArray))
                {
                    return null;
                }

                List<StockPerformers> gainersList = [];

                for (int i = 0; i < 10; i++)
                {
                    var gainer = gainersArray[i];
                    var gainerObject = new StockPerformers
                    {
                        Ticker = GetJsonValue(gainer, "ticker", "Unknown"),
                        Price = GetJsonValue(gainer, "price", 0m),
                        ChangeAmount = GetJsonValue(gainer, "change_amount", 0m),
                        ChangePercentage = GetJsonValue(gainer, "change_percentage", 0m),
                        Volume = GetJsonValue(gainer, "volume", 0m)
                    };

                    gainersList.Add(gainerObject);
                }
                return gainersList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static T GetJsonValue<T>(JsonElement element, string propertyName, T defaultValue)
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
