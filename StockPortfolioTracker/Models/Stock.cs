namespace StockPortfolioTracker.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public String StockExchange { get; set; }
        public String Ticker { get; set; }
        public Int32 Shares { get; set; }
        public String Currency { get; set; }
        public Decimal AverageShareCost { get; set; }
    }
}
