namespace StockTracker.Models
{
    public class StockPerformers
    {
        public string? Ticker { get; set; }
        public decimal Price { get; set; }
        public decimal ChangeAmount { get; set; }
        public decimal ChangePercentage { get; set; }
        public decimal Volume { get; set; }
    }
}
