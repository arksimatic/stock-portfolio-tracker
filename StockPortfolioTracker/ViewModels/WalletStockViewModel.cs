﻿namespace StockPortfolioTracker.Models
{
    public class WalletStockViewModel
    {
        public Int32 Wallet_X_StockId { get; set; }
        public Int32 WalletId { get; set; }

        public Int32 StockId { get; set; }
        public String StockExchange { get; set; }
        public String Ticker { get; set; }

        public CurrencyCode WalletCurrencyCode { get; set; }
        public CurrencyCode CurrencyCode { get; set; }
        public Int32 Shares { get; set; }
        public DateTime BuyDateTime { get; set; }

        public Decimal AverageShareCost { get; set; }
        public Decimal CurrentShareValue { get; set; }
        public Decimal AverageTotalCost { get; set; }
        public Decimal CurrentTotalValue { get; set; }
        public Decimal DividendsSum { get; set; }

        public Decimal AverageShareCostInWalletCurrency { get; set; }
        public Decimal CurrentShareValueInWalletCurrency { get; set; }
        public Decimal AverageTotalCostInWalletCurrency { get; set; }
        public Decimal CurrentTotalValueInWalletCurrency { get; set; }
        public Decimal DividendsSumInWalletCurrency { get; set; }
    }
}
