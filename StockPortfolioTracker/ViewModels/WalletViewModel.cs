using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.ViewModels
{
    public class WalletViewModel
    {
        public int WalletId { get; set; }
        public string Name { get; set;}
        public WalletStockViewModel[] WalletStocks { get; private set; }
        public WalletViewModel(Wallet wallet, Wallet_X_Stock[] wallets_x_stocks, Stock[] stocks)
        {
            WalletId = wallet.Id;
            Name = wallet.Name;

            WalletStockViewModel[] walletStocks = new WalletStockViewModel[wallets_x_stocks.Length];
            for(int i = 0; i < wallets_x_stocks.Length; i++)
                walletStocks[i] = new WalletStockViewModel(wallets_x_stocks[i], stocks.Where(stock => stock.Id == wallets_x_stocks[i].StockId).First()); //TODO: what if stock doesn't exists?
            WalletStocks = walletStocks;
        }
    }
}