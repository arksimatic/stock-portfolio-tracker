using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Models;
using StockPortfolioTracker.ViewModels;

namespace StockPortfolioTracker.Controllers
{
    public class WalletsController : Controller
    {
        private readonly StockPortfolioTrackerContext _context;

        public WalletsController(StockPortfolioTrackerContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<WalletViewModel> walletsViewProxy = new();
            foreach (Wallet wallet in await _context.Wallet.ToListAsync())
            {
                var wallets_x_stocks = _context.Wallet_X_Stock.Where(wallet_x_stock => wallet_x_stock.WalletId == wallet.Id);
                var stocks = _context.Stock.Where(stock => wallets_x_stocks.Any(wallet_x_stock => wallet_x_stock.StockId == stock.Id));
                walletsViewProxy.Add(new WalletViewModel(wallet, wallets_x_stocks.ToArray(), stocks.ToArray()));
            }
            return View(walletsViewProxy);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Wallet wallet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wallet);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
                return Problem("Wallet doesn't exists");
            else
                return RedirectToWallet(id.Value);
            //if (id == null || _context.Wallet == null)
            //{
            //    return NotFound();
            //}

            //var wallet = await _context.Wallet
            //    .FirstOrDefaultAsync(m => m.Id == id);
            //if (wallet == null)
            //{
            //    return NotFound();
            //}

            //return View(wallet);
        }

        // Action to redirect to a specific wallet
        public IActionResult RedirectToWallet(int walletId)
        {
            // Redirect to WalletController's Details action with the specified wallet ID
            return RedirectToAction("Index", "Wallet", new { id = walletId });
        }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Wallet wallet)
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
