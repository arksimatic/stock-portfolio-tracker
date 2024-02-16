using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
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
            {
                var wallets_x_stocks = _context.Wallet_X_Stock.Where(wallet_x_stock => wallet_x_stock.WalletId == wallet.Id);
                var stocks = _context.Stock.Where(stock => wallets_x_stocks.Any(wallet_x_stock => wallet_x_stock.StockId == stock.Id));
                var dividends = _context.Dividend.Where(dividend => stocks.Any(stock => stock.Id == dividend.StockId)).ToArray();
                var currencies = _context.Currency.ToArray();
                walletsViewProxy.Add(new WalletViewModel(wallet, wallets_x_stocks.ToArray(), stocks.ToArray(), dividends, currencies));
            }
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
            var wallet = await _context.Wallet.FindAsync(id);
            var currencies =_context.Currency.ToArray();
            var walletViewModel = new WalletViewModel(wallet, new Wallet_X_Stock[0], new Stock[0], new Dividend[0], currencies);
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
