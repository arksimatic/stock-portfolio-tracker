using System.Globalization;
using Microsoft.EntityFrameworkCore;
using StockPortfolioTracker.Data;
using StockPortfolioTracker.Services.YahooApiService;
using Microsoft.AspNetCore.Identity;

// Set the culture
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StockPortfolioTrackerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StockPortfolioTrackerContext") ?? throw new InvalidOperationException("Connection string 'StockPortfolioTrackerContext' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<StockPortfolioTrackerContext>();

// Add StockDataService to update stock data
builder.Services.AddHostedService<StockDataService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
