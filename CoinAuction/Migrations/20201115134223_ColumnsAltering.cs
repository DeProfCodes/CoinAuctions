using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class ColumnsAltering : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Banks");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Wallets",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Banks",
                newName: "Id");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "Banks",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BankName",
                table: "Banks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Wallets",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Banks",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Banks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
