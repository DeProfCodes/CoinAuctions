using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class addmanualschedulefield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsManualScheduled",
                table: "Auctions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsManualScheduled",
                table: "Auctions");
        }
    }
}
