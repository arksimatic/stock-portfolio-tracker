namespace StockPortfolioTracker.Services.YahooApiService
{
    public interface IStockDataService
    {
        Task<StockExternalData> GetStockDataAsync(String stockExchange, String ticker);
    }
}
