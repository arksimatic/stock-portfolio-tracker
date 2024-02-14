using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Models;
using StockPortfolioTracker.Services.YahooApiService;
using StockPortfolioTracker.ViewModels;

namespace StockPortfolioTracker.Controllers
{
public class AxisLabelData
{
    public string x;
    public double y;
}
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
            var wallets = await _context.Wallet.ToListAsync();
            var wallet = wallets.Where(wallet => wallet.Id == walletId).FirstOrDefault();
            if (wallet != null)
            {
                var wallets_x_stocks = _context.Wallet_X_Stock.Where(wallet_x_stock => wallet_x_stock.WalletId == wallet.Id);
                var stocks = _context.Stock.Where(stock => wallets_x_stocks.Any(wallet_x_stock => wallet_x_stock.StockId == stock.Id));
                var dividends = _context.Dividend.Where(dividend => stocks.Any(stock => stock.Id == dividend.StockId)).ToArray();
                var currencies = _context.Currency.Where(currency => stocks.Any(stock => stock.CurrencyId == currency.Id)).ToArray();
                var walletViewProxy = new WalletViewModel(wallet, wallets_x_stocks.ToArray(), stocks.ToArray(), dividends, currencies);
                return View(walletViewProxy);
            }
            else
                return Problem("This wallet doesn't exists.");
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
            if (ModelState.IsValid)
            {
                var stock = _context.Stock.Where(stock => stock.Ticker == walletStockViewModel.Ticker && stock.StockExchange == walletStockViewModel.StockExchange).FirstOrDefault();
                if (stock == null)
                {
                    stock = new Stock()
                    {
                        StockExchange = walletStockViewModel.StockExchange,
                        Ticker = walletStockViewModel.Ticker,
                    };
                    _context.Add(stock);
                    await _context.SaveChangesAsync();
                }
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
            var walletStockViewModel = new WalletStockViewModel(wallet_x_stock, stock, dividends, currency);
            return View(walletStockViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WalletStockViewModel walletStockViewModel)
        {
            if (ModelState.IsValid)
            {
                var stock = _context.Stock.Where(stock => stock.Ticker == walletStockViewModel.Ticker && stock.StockExchange == walletStockViewModel.StockExchange).FirstOrDefault();
                if(stock != null) // TODO: what if actually null?
                {
                    var wallet_x_stock = await _context.Wallet_X_Stock.FindAsync(walletStockViewModel.Wallet_X_StockId);
                    wallet_x_stock.Shares = walletStockViewModel.Shares;
                    wallet_x_stock.AverageShareCost = walletStockViewModel.AverageShareCost;
                    wallet_x_stock.BuyDateTime = walletStockViewModel.BuyDateTime;
                    _context.Update(wallet_x_stock);
                    await _context.SaveChangesAsync();
                }

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
    }
}
