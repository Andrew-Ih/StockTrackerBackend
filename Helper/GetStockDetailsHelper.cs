using System.Text.Json;
using StockTracker.Models;

namespace StockTracker.Helper
{
    public static class GetStockDetailsHelper
    {
        private static readonly string? MarketStackStockDetailsApi = Environment.GetEnvironmentVariable("MarketStackStockDetailsApi");
        public static async Task<StockDetails?> GetStockObjectDetailsAsync(string requestBody)
        {
            StockDetails? stockObject = new();
            string? stockSymbol = ExtractStockSymbol(requestBody);

            if (string.IsNullOrEmpty(stockSymbol))
            {
                stockObject.IsNull = true;
                stockObject.ErrorMessage = "Error extracting stock symbol";
            }
            else
            {
                using HttpClient client = new();
                string apiUrl = MarketStackStockDetailsApi + stockSymbol;
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    stockObject = ProcessStockApiResponse(responseData);
                }
            }
            return stockObject;
        }

        public static string? ExtractStockSymbol(string requestBody)
        {
            var requestData = JsonSerializer.Deserialize<Dictionary<string, string>>(requestBody);
            return ValidateRequestData(requestData, out var stockSymbol) ? stockSymbol : null;
        }

        public static bool ValidateRequestData(Dictionary<string, string>? requestData, out string? stockSymbol)
        {
            stockSymbol = requestData?.GetValueOrDefault("stockSymbol");
            return !string.IsNullOrEmpty(stockSymbol);
        }

        public static StockDetails? ProcessStockApiResponse(string responseData)
        {
            StockDetails? stockObject = new();
            try
            {
                var stockData = JsonDocument.Parse(responseData);
                var stockEntries = stockData.RootElement.GetProperty("data");
                
                if (stockEntries.GetArrayLength() >= 2) // Ensure we have at least two days of data
                {
                    stockObject = ExtractStockDetails(stockEntries);
                }
                else
                {
                    stockObject.IsNull = true;
                    stockObject.ErrorMessage = "Not Enough Stock Data Available";
                }
            }
            catch (Exception)
            {
                if (stockObject != null) {
                    stockObject.IsNull = true;
                    stockObject.ErrorMessage = "Error Processing Stock Data";
                }
            }
            return stockObject;
        }

        public static StockDetails? ExtractStockDetails(JsonElement stockEntries)
        {
            StockDetails? stockObject = new();
            var currentDayStock = stockEntries[0];
            var previousDayStock = stockEntries[1];

            string? stockName = currentDayStock.GetProperty("name").GetString(); ;
            string? stockSymbol = currentDayStock.GetProperty("symbol").GetString();

            if (stockName != null && stockSymbol != null)
            {
                stockObject.StockName = stockName;
                stockObject.StockSymbol = stockSymbol;
                stockObject.Price = currentDayStock.GetProperty("close").GetDecimal();
                stockObject.DailyHigh = currentDayStock.GetProperty("high").GetDecimal();
                stockObject.DailyLow = currentDayStock.GetProperty("low").GetDecimal();
                stockObject.OpenPrice = currentDayStock.GetProperty("open").GetDecimal();
                stockObject.ClosePrice = currentDayStock.GetProperty("close").GetDecimal();
                stockObject.PreviousDayClosePrice = previousDayStock.GetProperty("close").GetDecimal();
                stockObject.DailyVolume = currentDayStock.GetProperty("volume").GetDecimal();
            }
            else
            {
                stockObject.IsNull = true;
                stockObject.ErrorMessage = "Error Extracting Stock Details";
            }
            return stockObject;
        }
    }
}
