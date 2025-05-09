using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using StockTracker.Models;

namespace StockTracker.Helper
{
    public static class GetBestOrWorstPerformingDailyStocksHelper
    {
        private static readonly string? AlphavantageStockPerformanceDetailsApi = Environment.GetEnvironmentVariable("AlphavantageStockPerformanceDetailsApi");

        public static async Task<List<TopOrWorstGainers>?> GetTopBestOrWorstPerformingStocksAsync(string stockType)
        {
            using (HttpClient client = new HttpClient())
            {
                string? apiUrl = AlphavantageStockPerformanceDetailsApi;
                HttpResponseMessage response = await client.GetAsync(apiUrl);
                List<TopOrWorstGainers>? result = null;

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    result = ProcessStockPerformaceData(responseData, stockType);
                }
                return result;
            }
        }

        public static List<TopOrWorstGainers>? ProcessStockPerformaceData(string responseData, string stockType)
        {
            try
            {
                var stockData = JsonDocument.Parse(responseData);

                // Ensure "top_gainers" property exists in JSON response
                if (!stockData.RootElement.TryGetProperty(stockType, out JsonElement gainersArray))
                {
                    return null;
                }

                List<TopOrWorstGainers> gainersList = [];

                for (int i = 0; i < 10; i++)
                {
                    var gainer = gainersArray[i];

                    string ticker = GetJsonValue(gainer, "ticker", "Unknown");
                    decimal price = GetJsonValue(gainer, "price", 0m);
                    decimal changeAmount = GetJsonValue(gainer, "change_amount", 0m);
                    decimal changePercentage = GetJsonValue(gainer, "change_percentage", 0m);
                    decimal volume = GetJsonValue(gainer, "volume", 0m);

                    gainersList.Add(new TopOrWorstGainers(ticker, price, changeAmount, changePercentage, volume));
                }

                return gainersList;
            }
            catch (Exception ex)
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
