namespace StockPortfolioTracker.Models
{
    public class Wallet_X_Stock
    {
        public Int32 Id { get; set; }
        //public Int32 UserId { get; set; }
        public Int32 WalletId { get; set; }
        public Int32 StockId { get; set; }
        public Int32 Shares { get; set; }
        public Decimal AverageShareCost { get; set; }
    }
}
