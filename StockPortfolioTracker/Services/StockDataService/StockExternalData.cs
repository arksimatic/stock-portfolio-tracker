namespace StockPortfolioTracker.Services.YahooApiService
{
    public class StockExternalData
    {
        public String StockExchange { get; set; } //TODO: Change to enum
        public String Ticker { get; set; } //TODO: Change to enum
        public Decimal CurrentShareValue { get; set; }
    }
}
