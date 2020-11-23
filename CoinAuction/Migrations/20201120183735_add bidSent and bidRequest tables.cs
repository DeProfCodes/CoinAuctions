using Microsoft.EntityFrameworkCore.Migrations;

namespace CoinAuction.Migrations
{
    public partial class addbidSentandbidRequesttables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bids");

            migrationBuilder.CreateTable(
                name: "BidsRequest",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidSentId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    BidderName = table.Column<int>(nullable: false),
                    BidderCellphone = table.Column<int>(nullable: false),
                    BidCoins = table.Column<int>(nullable: false),
                    BidStatus = table.Column<string>(nullable: true)
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
                    BidRequestId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    RecipientName = table.Column<int>(nullable: false),
                    Cellphone = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BranchCode = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    AccountType = table.Column<string>(nullable: true),
                    BidCoins = table.Column<int>(nullable: false),
                    BidStatus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidsSent", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidsRequest");

            migrationBuilder.DropTable(
                name: "BidsSent");

            migrationBuilder.CreateTable(
                name: "Bids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BidCoins = table.Column<int>(type: "int", nullable: false),
                    BidStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BidderCellphone = table.Column<int>(type: "int", nullable: false),
                    BidderName = table.Column<int>(type: "int", nullable: false),
                    FromUserId = table.Column<int>(type: "int", nullable: false),
                    ToUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bids", x => x.Id);
                });
        }
    }
}
