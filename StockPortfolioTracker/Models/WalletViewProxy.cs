namespace StockPortfolioTracker.Models
{
    public class WalletViewProxy
    {
        private Wallet _wallet;
        private Wallet_X_Stock[] _wallets_x_stocks;
        private Stock[] _stocks;
        public Int32 Id => _wallet.Id;
        public String Name => _wallet.Name;
        public WalletStockViewProxy[] WalletStocks { get; private set; }
        public WalletViewProxy(Wallet wallet, Wallet_X_Stock[] wallets_x_stocks, Stock[] stocks)
        {
            _wallet = wallet;
            _wallets_x_stocks = wallets_x_stocks;
            _stocks = stocks;

            WalletStockViewProxy[] walletStocks = new WalletStockViewProxy[_wallets_x_stocks.Length];
            for (Int32 i = 0; i < _wallets_x_stocks.Length; i++)
                walletStocks[i] = new WalletStockViewProxy(_wallets_x_stocks[i], _stocks[i]);
            WalletStocks = walletStocks;
        }
    }
}
