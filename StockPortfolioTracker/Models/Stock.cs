namespace StockPortfolioTracker.Models
{
    // Generally speaking, StockExchange and Ticker should be separate entities (enums?), but for simplicity I'm keeping them as strings.
    // Also that would mean that I would need to maintain them up to date, which is not really feasible, especially without a proper data source.
    public class Stock
    {
        public int Id { get; set; }
        public Int32 CurrencyId { get; set; }
        public String StockExchange { get; set; }
        public String Ticker { get; set; }
        public Decimal CurrentShareValue { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
    }
}
