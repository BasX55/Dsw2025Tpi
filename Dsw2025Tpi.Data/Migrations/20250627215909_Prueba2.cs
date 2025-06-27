using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dsw2025Tpi.Data.Migrations
{
    /// <inheritdoc />
    public partial class Prueba2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Product_ProductSku",
                table: "OrdersItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Product",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "Product",
                newName: "Products");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Sku");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Products_ProductSku",
                table: "OrdersItems",
                column: "ProductSku",
                principalTable: "Products",
                principalColumn: "Sku",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrdersItems_Products_ProductSku",
                table: "OrdersItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Product");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Product",
                table: "Product",
                column: "Sku");

            migrationBuilder.AddForeignKey(
                name: "FK_OrdersItems_Product_ProductSku",
                table: "OrdersItems",
                column: "ProductSku",
                principalTable: "Product",
                principalColumn: "Sku",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
