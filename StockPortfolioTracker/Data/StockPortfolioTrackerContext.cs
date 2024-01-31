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
        //public DbSet<StockPortfolioTracker.Models.User> User { get; set; } = default!;
        public DbSet<StockPortfolioTracker.Models.Wallet_X_Stock> Wallet_X_Stock { get; set; } = default!;
        public DbSet<StockPortfolioTracker.Models.Stock> Stock { get; set; } = default!;

        public DbSet<StockPortfolioTracker.Models.Test>? Test { get; set; }
    }
}
