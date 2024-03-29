﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.Data
{
    public class StockPortfolioTrackerContext : IdentityDbContext
    {
        public StockPortfolioTrackerContext (DbContextOptions<StockPortfolioTrackerContext> options)
            : base(options)
        {
        }

        public DbSet<Wallet> Wallet { get; set; } = default!;
        public DbSet<Wallet_X_Stock> Wallet_X_Stock { get; set; } = default!;
        public DbSet<Stock> Stock { get; set; } = default!;
        public DbSet<Dividend> Dividend { get; set; } = default!;
        public DbSet<Currency> Currency { get; set; } = default!;
    }

    public class ApplicationUserEntityConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.Id).ValueGeneratedOnAdd();
        }
    }
}
