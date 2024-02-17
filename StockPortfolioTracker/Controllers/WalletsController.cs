using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Helpers;
using StockPortfolioTracker.Models;
using StockPortfolioTracker.Services.YahooApiService;
using StockPortfolioTracker.ViewModels;
using YahooQuotesApi;

namespace StockPortfolioTracker.Controllers
{
    public class WalletsController : Controller
    {
        private readonly StockPortfolioTrackerContext _context;
        public WalletsController(StockPortfolioTrackerContext context)
        {
            _context = context;
        }

        #region Index
        public async Task<IActionResult> Index()
        {
            List<WalletViewModel> walletsViewProxy = new();
            foreach (Wallet wallet in await _context.Wallet.ToListAsync())
                walletsViewProxy.Add(WalletHelper.GetWalletViewModel(wallet.Id, _context));
            return View(walletsViewProxy);
        }
        #endregion Index

        #region Detailes
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return Problem("Wallet doesn't exists");
            else
                return RedirectToWallet(id.Value);
        }
        public IActionResult RedirectToWallet(int walletId)
        {
            return RedirectToAction("Index", "Wallet", new { walletId = walletId });
        }
        #endregion Detailes

        #region Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WalletViewModel walletViewModel)
        {
            ModelState.Remove("ChartData");
            ModelState.Remove("WalletStocks");
            if (ModelState.IsValid)
            {
                var wallet = new Wallet()
                {
                    Name = walletViewModel.Name,
                    DefaultCurrencyId = _context.Currency.Where(currency => currency.Code == walletViewModel.DefaultCurrencyCode).FirstOrDefault().Id
                };
                _context.Add(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(walletViewModel);
        }
        #endregion Create

        #region Edit
        public async Task<IActionResult> Edit(Int32 id)
        {
            var walletViewModel = WalletHelper.GetWalletViewModel(id, _context);
            return View(walletViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WalletViewModel walletViewModel)
        {
            ModelState.Remove("ChartData");
            ModelState.Remove("WalletStocks");
            if (ModelState.IsValid)
            {
                var wallet = new Wallet()
                {
                    Id = walletViewModel.WalletId,
                    Name = walletViewModel.Name,
                    DefaultCurrencyId = _context.Currency.Where(currency => currency.Code == walletViewModel.DefaultCurrencyCode).FirstOrDefault().Id
                };
                _context.Update(wallet);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(walletViewModel);
        }
        #endregion Edit

        #region Delete
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Wallet == null)
                return NotFound();

            var wallet = await _context.Wallet
                .FirstOrDefaultAsync(m => m.Id == id);
            if (wallet == null)
                return NotFound();

            _context.Wallet.Remove(wallet);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion Delete

        private bool WalletExists(int id)
        {
            return (_context.Wallet?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
