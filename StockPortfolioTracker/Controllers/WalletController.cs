using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.Controllers
{
    public class WalletController : Controller
    {
        private readonly StockPortfolioTrackerContext _context;

        public WalletController(StockPortfolioTrackerContext context)
        {
            _context = context;
        }

        // GET: Wallet
        public async Task<IActionResult> Index(Int32 walletId = 1)
        {
            var wallets = await _context.Wallet.ToListAsync();
            var wallet = wallets.Where(wallet => wallet.Id == walletId).FirstOrDefault();
            if (wallet != null)
            {
                    var wallets_x_stocks = _context.Wallet_X_Stock.Where(wallet_x_stock => wallet_x_stock.WalletId == wallet.Id);
                    var stocks = _context.Stock.Where(stock => wallets_x_stocks.Any(wallet_x_stock => wallet_x_stock.StockId == stock.Id));
                    var walletViewProxy = new WalletViewProxy(wallet, wallets_x_stocks.ToArray(), stocks.ToArray());
                    return View(walletViewProxy);
            }
            else
                return Problem("This wallet doesn't exists.");
        }

        // GET: Wallet/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // GET: Wallet/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Wallet/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] WalletStockViewProxy walletStockView)
        {
            if (ModelState.IsValid)
            {
                _context.Add(walletStockView);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(walletStockView);
        }

        // GET: Wallet/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet == null)
            {
                return NotFound();
            }
            return View(wallet);
        }

        // POST: Wallet/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Wallet wallet)
        {
            if (id != wallet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WalletExists(wallet.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(wallet);
        }

        // GET: Wallet/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wallet == null)
            {
                return NotFound();
            }

            var wallet = await _context.Wallet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
            {
                return NotFound();
            }

            return View(wallet);
        }

        // POST: Wallet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Wallet == null)
            {
                return Problem("Entity set 'StockPortfolioTrackerContext.Wallet'  is null.");
            }
            var wallet = await _context.Wallet.FindAsync(id);
            if (wallet != null)
            {
                _context.Wallet.Remove(wallet);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WalletExists(int id)
        {
          return (_context.Wallet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
