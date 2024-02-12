namespace StockPortfolioTracker.Models
{
    public class Currency
    {
        public Int32 Id { get; set; }
        public CurrencyCode Code { get; set; }
        public Decimal USDRatio { get; set; }
        public DateTime LastUpdateDateTime { get; set; }
    }
}
