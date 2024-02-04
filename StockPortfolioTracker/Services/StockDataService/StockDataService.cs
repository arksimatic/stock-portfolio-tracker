using YahooQuotesApi;

namespace StockPortfolioTracker.Services.YahooApiService
{
    public class StockDataService : IStockDataService
    {
        private readonly YahooQuotes _yahooQuotes;
        public StockDataService() 
        {
            _yahooQuotes = new YahooQuotesBuilder().Build();
        }
        public async Task<StockExternalData> GetStockDataAsync(String stockExchange, String ticker)
        {
            Security? security = await _yahooQuotes.GetAsync(ticker + "." + stockExchange);

            return new StockExternalData
            {
                StockExchange = stockExchange,
                Ticker = ticker,
                CurrentShareValue = security.RegularMarketPrice ?? 0
            };
        }
    }
}
