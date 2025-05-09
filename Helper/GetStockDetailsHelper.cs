using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StockTracker.Models;

namespace StockTracker.Helper
{
    public static class GetStockDetailsHelper
    {
        public static async Task<Stock?> GetStockObjectDetailsAsync(string requestBody)
        {
            var requestData = JsonSerializer.Deserialize<Dictionary<string, string>>(requestBody);

            // Ensure the request contains a valid stock symbol
            if (requestData == null || !requestData.TryGetValue("stockSymbol", out string? stockSymbol) || string.IsNullOrEmpty(stockSymbol))
            {
                Console.WriteLine("Stock symbol is missing or invalid.");
                return null;
            }

            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"http://api.marketstack.com/v2/eod?access_key=185793059cef5d26516a98175393f267&symbols={stockSymbol}";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Stock? stockObject = ReadStockResponse(responseData);

                    return stockObject;
                }
                else
                {
                    return null;
                }
            }
        }

        public static Stock? ReadStockResponse(string responseData)
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

                    // Extract required data
                    stockName = currentDayStock.GetProperty("name").GetString();
                    stockSymbol = currentDayStock.GetProperty("symbol").GetString();
                    decimal price = currentDayStock.GetProperty("close").GetDecimal();
                    decimal dailyHigh = currentDayStock.GetProperty("high").GetDecimal();
                    decimal dailyLow = currentDayStock.GetProperty("low").GetDecimal();
                    decimal openPrice = currentDayStock.GetProperty("open").GetDecimal();
                    decimal closePrice = currentDayStock.GetProperty("close").GetDecimal();
                    decimal previousClosePrice = previousDayStock.GetProperty("close").GetDecimal();
                    decimal dailyVolume = currentDayStock.GetProperty("volume").GetDecimal();

                    // Create Stock object
                    if (stockName != null && stockSymbol != null)
                    {
                        return new Stock(stockName, stockSymbol, price, dailyHigh, dailyLow, openPrice, closePrice, previousClosePrice, dailyVolume);
                    } else
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
