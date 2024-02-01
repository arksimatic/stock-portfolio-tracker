namespace StockPortfolioTracker.Models
{
    public class WalletStockViewModel
    {
        private Wallet_X_Stock _wallet_x_stock;
        private Stock _stock;
        public Int32 Id => _wallet_x_stock?.Id ?? 0;
        public String StockExchange { get; set; }
        public String Ticker { get; set; }
        public String Currency { get; set; }
        public Int32 Shares { get; set; }
        public Decimal AverageShareCost { get; set;}
        public String DisplayText => $"{StockExchange}: {Ticker} \r\n {Shares} shared of average cost {AverageShareCost} {Currency} \r\n Total value: {Shares * AverageShareCost} {Currency}";
        public WalletStockViewModel() { }
        public WalletStockViewModel(Wallet_X_Stock wallet_x_stock, Stock stock)
        {
            _wallet_x_stock = wallet_x_stock;
            _stock = stock;

            StockExchange = _stock.StockExchange;
            Ticker = _stock.Ticker;
            Currency = _stock.Currency;
            Shares = _wallet_x_stock.Shares;
            AverageShareCost = _wallet_x_stock.AverageShareCost;
        }
    }
}
