using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace StockTracker.Models
{
    public class Stock
    {
        public string StockName { get; set; }
        public string StockSymbol { get; set; }
        public decimal Price { get; set; }
        public decimal DailyHigh { get; set; }
        public decimal DailyLow { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal PreviousDayClosePrice { get; set; }
        public decimal DailyVolume { get; set; }

        public Stock(string stockName, string stockSymbol, decimal price, decimal dailyHigh, decimal dailyLow, decimal openPrice, decimal closePrice,decimal previousDayClosePrice, decimal dailyVolume)
        {
            StockName = stockName;
            StockSymbol = stockSymbol;
            Price = price;
            DailyHigh = dailyHigh;
            DailyLow = dailyLow;
            OpenPrice = openPrice;
            ClosePrice = closePrice;
            PreviousDayClosePrice = previousDayClosePrice;
            DailyVolume = dailyVolume;
        }
    }
}
