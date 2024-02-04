using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StockPortfolioTracker.Migrations
{
    public partial class expandedstockmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentShareValue",
                table: "Stock",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDateTime",
                table: "Stock",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentShareValue",
                table: "Stock");

            migrationBuilder.DropColumn(
                name: "LastUpdateDateTime",
                table: "Stock");
        }
    }
}
