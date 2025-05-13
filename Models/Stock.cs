namespace StockTracker.Models
{
    public class StockDetails
    {
        public string? StockName { get; set; }
        public string? StockSymbol { get; set; }
        public decimal Price { get; set; }
        public decimal DailyHigh { get; set; }
        public decimal DailyLow { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal ClosePrice { get; set; }
        public decimal PreviousDayClosePrice { get; set; }
        public decimal DailyVolume { get; set; }
        public bool IsNull { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
