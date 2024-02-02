namespace StockPortfolioTracker.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public String StockExchange { get; set; } //TODO: Change to enum
        public String Ticker { get; set; } //TODO: Change to enum
        //public String Currency { get; set; } //TODO: Change to enum
    }
}
