using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Helpers;
using StockPortfolioTracker.Models;
using StockPortfolioTracker.ViewModels;

namespace StockPortfolioTracker.Controllers
{
public class WalletController : Controller
    {
        private readonly StockPortfolioTrackerContext _context;
        public WalletController(StockPortfolioTrackerContext context)
        {
            _context = context;
        }

        #region Index
        public async Task<IActionResult> Index(Int32 walletId)
        {
            WalletViewModel walletViewProxy = WalletHelper.GetWalletViewModel(walletId, _context);
            return View(walletViewProxy);
        }
        #endregion Index

        #region Create
        public IActionResult Create(Int32 walletId)
        {
            return View(new WalletStockViewModel() { WalletId = walletId});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WalletStockViewModel walletStockViewModel)
        {
            ModelState.Remove("Currency");
            ModelState.Remove("WalletCurrency");
            ModelState.Remove("WalletCurrencyCode");
            if (ModelState.IsValid)
            {
                var stock = await SaveStock(walletStockViewModel.Ticker, walletStockViewModel.StockExchange, walletStockViewModel.CurrencyCode);
                Wallet_X_Stock wallet_X_tock = new Wallet_X_Stock()
                {
                    WalletId = walletStockViewModel.WalletId,
                    StockId = stock.Id,
                    Shares = walletStockViewModel.Shares,
                    BuyDateTime = walletStockViewModel.BuyDateTime,
                    AverageShareCost = walletStockViewModel.AverageShareCost
                };
                _context.Add(wallet_X_tock);
                await _context.SaveChangesAsync();
                return RedirectToWallet(walletStockViewModel.WalletId);
            }
            return View(walletStockViewModel);
        }
        #endregion Create

        #region Update
        public async Task<IActionResult> Edit(Int32 wallet_x_stockId)
        {
            var wallet_x_stock = await _context.Wallet_X_Stock.FindAsync(wallet_x_stockId);
            var stock = await _context.Stock.FindAsync(wallet_x_stock.StockId); //TODO: what if stock doesn't exists?
            var dividends = await _context.Dividend.Where(dividend => dividend.StockId == stock.Id).ToArrayAsync();
            var currency = (await _context.Currency.Where(currency => currency.Id == stock.CurrencyId).ToArrayAsync()).FirstOrDefault();
            var walletStockViewModel = WalletHelper.GetWalletStockViewModel(wallet_x_stock.Id, _context);
            return View(walletStockViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WalletStockViewModel walletStockViewModel)
        {
            ModelState.Remove("Currency");
            ModelState.Remove("WalletCurrency");
            ModelState.Remove("WalletCurrencyCode");
            if (ModelState.IsValid)
            {
                var stock = await SaveStock(walletStockViewModel.Ticker, walletStockViewModel.StockExchange, walletStockViewModel.CurrencyCode);
                var wallet_x_stock = await _context.Wallet_X_Stock.FindAsync(walletStockViewModel.Wallet_X_StockId);
                wallet_x_stock.Shares = walletStockViewModel.Shares;
                wallet_x_stock.AverageShareCost = walletStockViewModel.AverageShareCost;
                wallet_x_stock.BuyDateTime = walletStockViewModel.BuyDateTime;
                wallet_x_stock.StockId = stock.Id;
                _context.Update(wallet_x_stock);
                await _context.SaveChangesAsync();

                return RedirectToWallet(walletStockViewModel.WalletId);
            }
            return View(walletStockViewModel);
        }
        #endregion Update

        #region Delete
        public async Task<IActionResult> Delete(int? wallet_x_stockId)
        {
            if (wallet_x_stockId == null || _context.Wallet_X_Stock == null)
                return NotFound();

            var wallet_x_stock = await _context.Wallet_X_Stock
                .FirstOrDefaultAsync(m => m.Id == wallet_x_stockId);
            if (wallet_x_stock == null)
                return NotFound();

            _context.Wallet_X_Stock.Remove(wallet_x_stock);
            await _context.SaveChangesAsync();
            return RedirectToWallet(wallet_x_stock.WalletId);
        }
        #endregion Delete

        public IActionResult RedirectToWallet(Int32 walletId)
        {
            return RedirectToAction("Index", "Wallet", new { walletId });
        }

        public async Task<Stock> SaveStock(String ticker, String stockExchange, String currencyCodeStr)
        {
            var currency = _context.Currency.Where(currency => currency.Code == (CurrencyCode)Enum.Parse(typeof(CurrencyCode), currencyCodeStr)).FirstOrDefault();
            var stock = _context.Stock.Where(stock => stock.Ticker == ticker && stock.StockExchange == stockExchange && stock.CurrencyId == currency.Id).FirstOrDefault();
            if (stock == null)
            {
                stock = new Stock()
                {
                    StockExchange = stockExchange,
                    Ticker = ticker,
                    CurrencyId = currency.Id
                };
                _context.Add(stock);
                await _context.SaveChangesAsync();
            }

            return stock;
        }
    }
}
