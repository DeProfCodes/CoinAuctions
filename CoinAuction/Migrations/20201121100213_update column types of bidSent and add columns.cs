using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class updatecolumntypesofbidSentandaddcolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BidderName",
                table: "BidsRequest",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "BidderCellphone",
                table: "BidsRequest",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "BidType",
                table: "BidsRequest",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BidType",
                table: "BidsRequest");

            migrationBuilder.AlterColumn<int>(
                name: "BidderName",
                table: "BidsRequest",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BidderCellphone",
                table: "BidsRequest",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
