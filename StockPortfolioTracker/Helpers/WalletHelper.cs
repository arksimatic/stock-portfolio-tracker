using Microsoft.OpenApi.Services;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Models;
using StockPortfolioTracker.ViewModels;
using System.Xml.Linq;

namespace StockPortfolioTracker.Helpers
{
    public static class WalletHelper
    {
        public static WalletStockViewModel GetWalletStockViewModel(Int32 wallet_x_stockId, StockPortfolioTrackerContext context)
        {
            Wallet_X_Stock wallet_x_stock = context.Wallet_X_Stock.Find(wallet_x_stockId);
            Wallet wallet = context.Wallet.Find(wallet_x_stock.WalletId);
            Stock stock = context.Stock.Find(wallet_x_stock.StockId);

            Dividend[] dividends = context.Dividend.Where(dividend => dividend.StockId == stock.Id).ToArray();
            Currency stockCurrency = context.Currency.Where(currency => currency.Id == stock.CurrencyId).FirstOrDefault();
            Currency walletCurrency = context.Currency.Where(currency => currency.Id == wallet.DefaultCurrencyId).FirstOrDefault();
            
            Decimal dividendPerShare = dividends.DefaultIfEmpty().Where(dividend => dividend != null).Where(dividend => wallet_x_stock.BuyDateTime < dividend.DividendDate).Sum(dividend => dividend.DividendValue);

            var walletStockViewModel = new WalletStockViewModel()
            {
                Wallet_X_StockId = wallet_x_stock.Id,
                WalletId = wallet_x_stock.WalletId,
                StockId = wallet_x_stock.StockId,

                StockExchange = stock.StockExchange,
                Ticker = stock.Ticker,
                Shares = wallet_x_stock.Shares,
                Currency = stockCurrency,
                CurrencyCode = stockCurrency.Code.ToString(),
                WalletCurrency = walletCurrency,
                BuyDateTime = wallet_x_stock.BuyDateTime,

                AverageShareCost = wallet_x_stock.AverageShareCost,
                CurrentShareValue = stock.CurrentShareValue,
                AverageTotalCost = wallet_x_stock.AverageShareCost * wallet_x_stock.Shares,
                CurrentTotalValue = stock.CurrentShareValue * wallet_x_stock.Shares,
                DividendsSum = dividendPerShare * wallet_x_stock.Shares,

                AverageShareCostInWalletCurrency = CurrencyHelper.CalculateCurrencyValue(wallet_x_stock.AverageShareCost, stockCurrency, walletCurrency),
                CurrentShareValueInWalletCurrency = CurrencyHelper.CalculateCurrencyValue(stock.CurrentShareValue, stockCurrency, walletCurrency),
                AverageTotalCostInWalletCurrency = CurrencyHelper.CalculateCurrencyValue(wallet_x_stock.AverageShareCost * wallet_x_stock.Shares, stockCurrency, walletCurrency),
                CurrentTotalValueInWalletCurrency = CurrencyHelper.CalculateCurrencyValue(stock.CurrentShareValue * wallet_x_stock.Shares, stockCurrency, walletCurrency),
                DividendSumInWalletCurrency = CurrencyHelper.CalculateCurrencyValue(dividendPerShare * wallet_x_stock.Shares, stockCurrency, walletCurrency)
            };

            return walletStockViewModel;
        }
        
        public static WalletViewModel GetWalletViewModel(Int32 walletId, StockPortfolioTrackerContext context)
        {
            Wallet wallet = context.Wallet.Find(walletId);
            var wallets_x_stocks = context.Wallet_X_Stock.Where(wallet_x_stock => wallet_x_stock.WalletId == walletId);
            var stocks = context.Stock.Where(stock => wallets_x_stocks.Any(wallet_x_stock => wallet_x_stock.StockId == stock.Id));
            var dividends = context.Dividend.Where(dividend => stocks.Any(stock => stock.Id == dividend.StockId)).ToArray();
            Currency[] currencies = context.Currency.ToArray();
            Currency defaultCurrency = currencies.Where(currency => currency.Id == wallet.DefaultCurrencyId).FirstOrDefault();

            WalletStockViewModel[] walletStockViewModels = wallets_x_stocks.Select(wallet_x_stock => GetWalletStockViewModel(wallet_x_stock.Id, context)).ToArray();

            List<ChartData> chartData = new List<ChartData>();
            foreach (var walletStock in walletStockViewModels)
                chartData.Add(new ChartData { StockName = walletStock.Ticker, StockValue = walletStock.CurrentTotalValue });

            WalletViewModel walletViewModel = new WalletViewModel()
            {
                WalletId = wallet.Id,
                Name = wallet.Name,
                DefaultCurrencyCode = defaultCurrency.Code,
                
                WalletStocks = walletStockViewModels,

                CurrentValue = walletStockViewModels.Sum(walletStock => walletStock.CurrentTotalValue),
                CostValue = walletStockViewModels.Sum(walletStock => walletStock.AverageTotalCost),
                DividendsSum = walletStockViewModels.Sum(walletStock => walletStock.DividendsSum),

                CurrentValueInWalletCurrency = walletStockViewModels.Sum(walletStock => walletStock.CurrentTotalValueInWalletCurrency),
                CostValueInWalletCurrency = walletStockViewModels.Sum(walletStock => walletStock.AverageTotalCostInWalletCurrency),
                DividendsSumInWalletCurrency = walletStockViewModels.Sum(walletStock => walletStock.DividendSumInWalletCurrency),

                ChartData = chartData
            };

            return walletViewModel;
        }
    }
}
