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

        public DbSet<Wallet> Wallet { get; set; } = default!;
        public DbSet<Wallet_X_Stock> Wallet_X_Stock { get; set; } = default!;
        public DbSet<Stock> Stock { get; set; } = default!;
        public DbSet<Dividend> Dividend { get; set; } = default!;
        public DbSet<Test> Test { get; set; } = default!;

        //public DbSet<User> User { get; set; } = default!;
    }
}
