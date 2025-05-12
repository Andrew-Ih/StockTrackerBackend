using System.Text.Json;
using StockTracker.Models;

namespace StockTracker.Helper
{
    public static class GetStockDetailsHelper
    {
        private static readonly string? MarketStackStockDetailsApi = Environment.GetEnvironmentVariable("MarketStackStockDetailsApi");
        public static async Task<StockDetails?> GetStockObjectDetailsAsync(string requestBody)
        {
            string? stockSymbol = ExtractStockSymbol(requestBody);

            if (string.IsNullOrEmpty(stockSymbol))
            {
                return null;
            }

            using HttpClient client = new();
            string apiUrl = MarketStackStockDetailsApi + stockSymbol;
            HttpResponseMessage response = await client.GetAsync(apiUrl);
            StockDetails? stockObject = null;

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                stockObject = ProcessStockApiResponse(responseData);
            }

            return stockObject;
        }
        public static string? ExtractStockSymbol(string requestBody)
        {
            var requestData = JsonSerializer.Deserialize<Dictionary<string, string>>(requestBody);

            // Ensure the request contains a valid stock symbol
            if (requestData == null || !requestData.TryGetValue("stockSymbol", out string? stockSymbol) || string.IsNullOrEmpty(stockSymbol))
            {
                return null;
            }

            return stockSymbol;
        }

        public static StockDetails? ProcessStockApiResponse(string responseData)
        {
            try
            {
                var stockData = JsonDocument.Parse(responseData);
                var stockEntries = stockData.RootElement.GetProperty("data");
                string? stockName = "";
                string? stockSymbol = "";

                if (stockEntries.GetArrayLength() >= 2) // Ensure we have at least two days of data
                {
                    var currentDayStock = stockEntries[0];
                    var previousDayStock = stockEntries[1];

                    stockName = currentDayStock.GetProperty("name").GetString();
                    stockSymbol = currentDayStock.GetProperty("symbol").GetString();
                    decimal price = currentDayStock.GetProperty("close").GetDecimal();
                    decimal dailyHigh = currentDayStock.GetProperty("high").GetDecimal();
                    decimal dailyLow = currentDayStock.GetProperty("low").GetDecimal();
                    decimal openPrice = currentDayStock.GetProperty("open").GetDecimal();
                    decimal closePrice = currentDayStock.GetProperty("close").GetDecimal();
                    decimal previousClosePrice = previousDayStock.GetProperty("close").GetDecimal();
                    decimal dailyVolume = currentDayStock.GetProperty("volume").GetDecimal();

                    if (stockName != null && stockSymbol != null)
                    {
                        return new StockDetails
                        {
                            StockName = stockName, StockSymbol = stockSymbol, Price = price, DailyHigh = dailyHigh, DailyLow = dailyLow,
                            OpenPrice = openPrice, ClosePrice = closePrice, PreviousDayClosePrice = previousClosePrice, DailyVolume = dailyVolume
                        };
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    throw new Exception("Not enough data available.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing stock data: {ex.Message}");
                return null;
            }
        }
    }
}
