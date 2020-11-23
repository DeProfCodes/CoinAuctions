using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class changebankcolumntypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BranchCode",
                table: "Banks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AccountNumber",
                table: "Banks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BranchCode",
                table: "Banks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "AccountNumber",
                table: "Banks",
                type: "int",
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
