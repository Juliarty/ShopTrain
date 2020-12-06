using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Database.Migrations
{
    public partial class ChangedStocksOnHoldAddedSessionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "StocksOnHold",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StocksOnHold_StockId",
                table: "StocksOnHold",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_StocksOnHold_Stock_StockId",
                table: "StocksOnHold",
                column: "StockId",
                principalTable: "Stock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StocksOnHold_Stock_StockId",
                table: "StocksOnHold");

            migrationBuilder.DropIndex(
                name: "IX_StocksOnHold_StockId",
                table: "StocksOnHold");

            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "StocksOnHold");
        }
    }
}
