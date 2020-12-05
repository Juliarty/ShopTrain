using Microsoft.EntityFrameworkCore.Migrations;

namespace Shop.Database.Migrations
{
    public partial class ChangedTableRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStocks_Products_ProductId",
                table: "OrderStocks");

            migrationBuilder.DropIndex(
                name: "IX_OrderStocks_ProductId",
                table: "OrderStocks");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OrderStocks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OrderStocks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderStocks_ProductId",
                table: "OrderStocks",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStocks_Products_ProductId",
                table: "OrderStocks",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
