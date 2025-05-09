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
        public static async Task<List<TopOrWorstGainers>?> GetTopBestOrWorstPerformingStocksAsync(string stockType)
        {
            using (HttpClient client = new HttpClient())
            {
                string apiUrl = $"https://www.alphavantage.co/query?function=TOP_GAINERS_LOSERS&apikey=JCNUVCJ6WID61HZR";
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    return ReadTopGainersResponse(responseData, stockType);
                }
                else
                {
                    return null;
                }
            }
        }

        public static List<TopOrWorstGainers>? ReadTopGainersResponse(string responseData, string stockType)
        {
            try
            {
                var stockData = JsonDocument.Parse(responseData);

                // Ensure "top_gainers" property exists in JSON response
                if (!stockData.RootElement.TryGetProperty(stockType, out JsonElement gainersArray) || gainersArray.GetArrayLength() < 10)
                {
                    Console.WriteLine("Not enough top gainers data available.");
                    return null;
                }

                List<TopOrWorstGainers> gainersList = new List<TopOrWorstGainers>();

                // Loop through the top 10 stocks in the array
                for (int i = 0; i < Math.Min(10, gainersArray.GetArrayLength()); i++)
                {
                    var gainer = gainersArray[i];

                    string ticker = gainer.TryGetProperty("ticker", out var tickerElement) && tickerElement.ValueKind == JsonValueKind.String ? tickerElement.GetString() ?? "Unknown" : "Unknown";
                    decimal price = gainer.TryGetProperty("price", out var priceElement) && priceElement.ValueKind == JsonValueKind.String ? Convert.ToDecimal(priceElement.GetString()) : 0;
                    decimal changeAmount = gainer.TryGetProperty("change_amount", out var changeAmountElement) && changeAmountElement.ValueKind == JsonValueKind.String ? Convert.ToDecimal(changeAmountElement.GetString()) : 0;
                    decimal changePercentage = gainer.TryGetProperty("change_percentage", out var changePercentageElement) && changePercentageElement.ValueKind == JsonValueKind.String ? Convert.ToDecimal(changePercentageElement.GetString().Replace("%", "")) : 0;
                    decimal volume = gainer.TryGetProperty("volume", out var volumeElement) && volumeElement.ValueKind == JsonValueKind.String ? Convert.ToDecimal(volumeElement.GetString()) : 0;


                    // Create and add the object to the list
                    gainersList.Add(new TopOrWorstGainers(ticker, price, changeAmount, changePercentage, volume));
                }

                return gainersList;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing top gainers data: {ex.Message}");
                return null;
            }
        }
    }
}
