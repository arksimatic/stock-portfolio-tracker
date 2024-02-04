using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Models;
using YahooQuotesApi;

namespace StockPortfolioTracker.Services.YahooApiService
{
    public class StockDataService : IHostedService
    {
        private readonly YahooQuotes _yahooQuotes;
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;
        public StockDataService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _yahooQuotes = new YahooQuotesBuilder().Build();
        }
        public async Task UpdateStocksDataAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StockPortfolioTrackerContext>();
                var stocks = await dbContext.Stock.ToArrayAsync();
                stocks = stocks.Where(stock => stock.LastUpdateDateTime.Date < DateTime.Today).ToArray();
                IEnumerable<String> stockSymbols = stocks.Select(stock => stock.Ticker + "." + stock.StockExchange);

                Dictionary<String, Security?> securities = await _yahooQuotes.GetAsync(stockSymbols);
                Dictionary<Stock, Security?> stockSecurities = stocks.ToDictionary(stock => stock, stock => securities[stock.Ticker + "." + stock.StockExchange]);

                foreach (var stockSecurity in stockSecurities)
                {
                    Stock stock = stockSecurity.Key;
                    Security security = stockSecurity.Value;
                    if (security != null)
                    {
                        stock.CurrentShareValue = security.RegularMarketPrice ?? 0;
                        stock.LastUpdateDateTime = DateTime.Now;
                        dbContext.Stock.Update(stock);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async _ => await UpdateStocksDataAsync(), null, TimeSpan.Zero, TimeSpan.FromHours(1));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            _timer?.Dispose();
            _timer = null;

            return Task.CompletedTask;
        }
    }
}
