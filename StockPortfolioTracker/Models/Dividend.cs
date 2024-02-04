namespace StockPortfolioTracker.Models
{
    public class Dividend
    {
        public Int32 Id { get; set; }
        public Int32 StockId { get; set; }
        public DateTime DividendDate { get; set; }
        public Decimal DividendValue { get; set; }
    }
}
