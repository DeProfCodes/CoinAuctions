using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(5)",
                oldMaxLength: 5);

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

            migrationBuilder.CreateTable(
                name: "Auctions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    TotalPoolCoins = table.Column<int>(nullable: false),
                    TotalSoldCoins = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsManualScheduled = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Auctions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BidsRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidSentId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    BidderName = table.Column<string>(nullable: true),
                    RecipientName = table.Column<string>(nullable: true),
                    BidderCellphone = table.Column<string>(nullable: true),
                    BidCoins = table.Column<int>(nullable: false),
                    BidStatus = table.Column<string>(nullable: true),
                    BidType = table.Column<string>(nullable: true),
                    BidDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidsRequest", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BidsSent",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestUsersId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RecipientName = table.Column<string>(nullable: true),
                    Cellphone = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BranchCode = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    AccountType = table.Column<string>(nullable: true),
                    BidCoins = table.Column<int>(nullable: false),
                    BidCoinsType = table.Column<string>(nullable: true),
                    BidStatus = table.Column<string>(nullable: true),
                    BidDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidsSent", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coins",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(nullable: false),
                    OwnerName = table.Column<string>(nullable: true),
                    OpeningCoins = table.Column<int>(nullable: false),
                    ProfitCoins = table.Column<int>(nullable: false),
                    TotalCoins = table.Column<int>(nullable: false),
                    MaturationType = table.Column<string>(nullable: true),
                    MaturityDate = table.Column<DateTime>(nullable: false),
                    MaturityStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coins", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Auctions");

            migrationBuilder.DropTable(
                name: "BidsRequest");

            migrationBuilder.DropTable(
                name: "BidsSent");

            migrationBuilder.DropTable(
                name: "Coins");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2147483647);

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
