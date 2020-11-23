using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class addCoinscolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OpeningCoins",
                table: "Coins",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProfitCoins",
                table: "Coins",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpeningCoins",
                table: "Coins");

            migrationBuilder.DropColumn(
                name: "ProfitCoins",
                table: "Coins");
        }
    }
}
