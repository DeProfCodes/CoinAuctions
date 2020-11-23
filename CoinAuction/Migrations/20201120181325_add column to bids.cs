using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class addcolumntobids : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Bids");

            migrationBuilder.AddColumn<int>(
                name: "FromUserId",
                table: "Bids",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToUserId",
                table: "Bids",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromUserId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "ToUserId",
                table: "Bids");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Bids",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
