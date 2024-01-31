namespace StockPortfolioTracker.Models
{
    public class WalletStockViewProxy
    {
        private Wallet_X_Stock _wallet_x_stock;
        private Stock _stock;
        public Int32 Id => _wallet_x_stock.Id;
        public String StockExchange => _stock.StockExchange;
        public String Ticker => _stock.Ticker;
        public String Currency => _stock.Currency;
        public Int32 Shares => _wallet_x_stock.Shares;
        public Decimal AverageShareCost => _wallet_x_stock.AverageShareCost;
        public WalletStockViewProxy(Wallet_X_Stock wallet_x_stock, Stock stock)
        {
            _wallet_x_stock = wallet_x_stock;
            _stock = stock;
        }
        public String Text => $"{StockExchange}: {Ticker} \r\n {Shares} shared of average cost {AverageShareCost} {Currency} \r\n Total value: {Shares * AverageShareCost} {Currency}";
    }
}
