using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockPortfolioTracker.Migrations
{
    public partial class addedcurrencytostock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Stock",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Stock");
        }
    }
}
