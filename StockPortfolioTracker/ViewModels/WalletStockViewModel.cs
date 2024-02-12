using StockPortfolioTracker.Services.YahooApiService;

namespace StockPortfolioTracker.Models
{
    public class WalletStockViewModel
    {
        public Int32 Wallet_X_StockId { get; set; }
        public Int32 WalletId { get; set; }
        public Int32 StockId { get; set; }
        public String StockExchange { get; set; }
        public String Ticker { get; set; }
        public Int32 Shares { get; set; }
        public DateTime BuyDateTime { get; set; }
        public Decimal AverageShareCost { get; set; }
        public Decimal CurrentShareValue { get; set; }
        public Decimal AverageTotalCost { get; set; }
        public Decimal CurrentTotalValue { get; set; }
        public Decimal DividendsSum { get; set; }
        public String DisplayText => $"{StockExchange}: {Ticker} \r\n {Shares} shares of average cost {AverageShareCost} PLN \r\n Total value: {Shares * AverageShareCost} PLN"; //TODO: change pln
        public WalletStockViewModel() { }
        public WalletStockViewModel(Wallet_X_Stock wallet_x_stock, Stock stock, Dividend[] dividends)
        {
            Wallet_X_StockId = wallet_x_stock.Id;
            WalletId = wallet_x_stock.WalletId;
            StockId = wallet_x_stock.StockId;

            Shares = wallet_x_stock.Shares;
            AverageShareCost = wallet_x_stock.AverageShareCost;
            CurrentShareValue = stock.CurrentShareValue;
            BuyDateTime = wallet_x_stock.BuyDateTime;
            Decimal dividendPerShare = dividends.DefaultIfEmpty().Where(dividend => dividend != null).Where(dividend => wallet_x_stock.BuyDateTime < dividend.DividendDate).Sum(dividend => dividend.DividendValue);
            DividendsSum = dividendPerShare * wallet_x_stock.Shares;

            AverageTotalCost = wallet_x_stock.AverageShareCost * wallet_x_stock.Shares;
            CurrentTotalValue = stock.CurrentShareValue * wallet_x_stock.Shares;

            StockExchange = stock.StockExchange;
            Ticker = stock.Ticker;
        }
    }
}
