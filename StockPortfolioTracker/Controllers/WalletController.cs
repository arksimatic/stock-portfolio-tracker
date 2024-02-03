using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Models;
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
                var walletViewProxy = new WalletViewModel(wallet, wallets_x_stocks.ToArray(), stocks.ToArray());
                List<AxisLabelData> chartData = new List<AxisLabelData>
            {
                new AxisLabelData { x= "South Korea", y= 39.4 },
                new AxisLabelData { x= "India", y= 61.3 },
                new AxisLabelData { x= "Pakistan", y= 20.4 },
                new AxisLabelData { x= "Germany", y= 65.1 },
                new AxisLabelData { x= "Australia", y= 15.8 },
                new AxisLabelData { x= "Italy", y= 29.2 },
                new AxisLabelData { x= "United Kingdom", y= 44.6 },
                new AxisLabelData { x= "Saudi Arabia", y= 9.7 },
                new AxisLabelData { x= "Russia", y= 40.8 },
                new AxisLabelData { x= "Mexico", y= 31 },
                new AxisLabelData { x= "Brazil", y= 75.9 },
                new AxisLabelData { x= "China", y= 51.4 }
            };
                ViewBag.dataSource = chartData;
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
            var walletStockViewModel = new WalletStockViewModel(wallet_x_stock, stock);
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
                    _context.Update(wallet_x_stock);
                    await _context.SaveChangesAsync();
                }

                return RedirectToWallet(walletStockViewModel.WalletId);
            }
            return View(walletStockViewModel);
        }
        #endregion Update

        #region Delete
        public async Task<IActionResult> Delete(Int32 wallet_x_stockId)
        {
            var wallet_x_stock = await _context.Wallet_X_Stock.FindAsync(wallet_x_stockId);
            var stock = await _context.Stock.FindAsync(wallet_x_stock.StockId); //TODO: what if stock doesn't exists?
            var walletStockViewModel = new WalletStockViewModel(wallet_x_stock, stock);
            return View(walletStockViewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(WalletStockViewModel walletStockViewModel)
        {
            var wallet_x_stock = await _context.Wallet_X_Stock.FindAsync(walletStockViewModel.Wallet_X_StockId);
            _context.Wallet_X_Stock.Remove(wallet_x_stock);
            await _context.SaveChangesAsync();
            return RedirectToWallet(walletStockViewModel.WalletId);
        }
        #endregion Delete

        public IActionResult RedirectToWallet(Int32 walletId)
        {
            return RedirectToAction("Index", "Wallet", new { walletId });
        }
        private bool WalletExists(int id)
        {
          return (_context.Wallet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
