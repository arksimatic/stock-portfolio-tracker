using Microsoft.AspNetCore.Mvc;

namespace StockPortfolioTracker.Controllers
{
    public class WalletsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
