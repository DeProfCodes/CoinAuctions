using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class changecolumnname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BidRequestId",
                table: "BidsSent");

            migrationBuilder.AddColumn<int>(
                name: "RequestUsersId",
                table: "BidsSent",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestUsersId",
                table: "BidsSent");

            migrationBuilder.AddColumn<int>(
                name: "BidRequestId",
                table: "BidsSent",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
