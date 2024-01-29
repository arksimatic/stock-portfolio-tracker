using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.Data
{
    public class StockPortfolioTrackerContext : DbContext
    {
        public StockPortfolioTrackerContext (DbContextOptions<StockPortfolioTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<StockPortfolioTracker.Models.Wallet> Wallet { get; set; } = default!;
    }
}
