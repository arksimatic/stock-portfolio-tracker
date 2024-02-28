using Microsoft.AspNetCore.Identity;

namespace StockPortfolioTracker.Models
{
    public class User : IdentityUser
    {
        public Int32 Id { get; set; }
    }
}
