using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockPortfolioTracker.Migrations
{
    public partial class updatecurrencyforexistingstocks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE Stock SET CurrencyId = 1 WHERE CurrencyId = 0");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
