using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.ViewModels
{
    public class ChartData
    {
        public String StockName { get; set; }
        public Decimal StockValue { get; set; }
    }
    public class WalletViewModel
    {
        public Int32 WalletId { get; set; }
        public String Name { get; set;}
        public Decimal CurrentValue { get; set; }
        public Decimal CostValue { get; set; }
        public WalletStockViewModel[] WalletStocks { get; private set; }
        public List<ChartData> ChartData { get; set; }
        public WalletViewModel(Wallet wallet, Wallet_X_Stock[] wallets_x_stocks, Stock[] stocks)
        {
            WalletId = wallet.Id;
            Name = wallet.Name;

            WalletStockViewModel[] walletStocks = new WalletStockViewModel[wallets_x_stocks.Length];
            for(int i = 0; i < wallets_x_stocks.Length; i++)
                walletStocks[i] = new WalletStockViewModel(wallets_x_stocks[i], stocks.Where(stock => stock.Id == wallets_x_stocks[i].StockId).First()); //TODO: what if stock doesn't exists?
            WalletStocks = walletStocks;

            CurrentValue = WalletStocks.Sum(walletStock => walletStock.Shares * walletStock.AverageShareCost);
            CostValue = WalletStocks.Sum(walletStock => walletStock.Shares * walletStock.AverageShareCost);

            List<ChartData> chartData = new List<ChartData>();
            foreach (var walletStock in WalletStocks)
                chartData.Add(new ChartData { StockName = walletStock.Ticker, StockValue = walletStock.Shares * walletStock.AverageShareCost });
            ChartData = chartData;
        }
    }
}