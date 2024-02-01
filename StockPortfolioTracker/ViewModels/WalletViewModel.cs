using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.ViewModels
{
    public class WalletViewModel
    {
        private Wallet _wallet;
        private Wallet_X_Stock[] _wallets_x_stocks;
        private Stock[] _stocks;
        public int Id => _wallet.Id;
        public string Name { get; set;}
        public WalletStockViewModel[] WalletStocks { get; private set; }
        public WalletViewModel(Wallet wallet, Wallet_X_Stock[] wallets_x_stocks, Stock[] stocks)
        {
            _wallet = wallet;
            _wallets_x_stocks = wallets_x_stocks;
            _stocks = stocks;

            Name = _wallet.Name;

            WalletStockViewModel[] walletStocks = new WalletStockViewModel[_wallets_x_stocks.Length];
            for (int i = 0; i < _wallets_x_stocks.Length; i++)
                walletStocks[i] = new WalletStockViewModel(_wallets_x_stocks[i], _stocks[i]);
            WalletStocks = walletStocks;
        }
    }
}
