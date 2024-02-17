using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Helpers;
using StockPortfolioTracker.Models;
using StockPortfolioTracker.Services.YahooApiService;

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
        public CurrencyCode DefaultCurrencyCode { get; set; }

        public Decimal CurrentValue { get; set; }
        public Decimal CostValue { get; set; }
        public Decimal DividendsSum { get; set; }

        public Decimal CurrentValueInWalletCurrency { get; set; }
        public Decimal CostValueInWalletCurrency { get; set; }
        public Decimal DividendsSumInWalletCurrency { get; set; }

        public WalletStockViewModel[] WalletStocks { get; set; }
        public List<ChartData> ChartData { get; set; }
    }
}
