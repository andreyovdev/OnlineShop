using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedPurchaseEntityWithQuantityAndPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Purchases");

            migrationBuilder.AddColumn<int>(
                name: "QuantityBought",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Quantity of the purchased product");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "Purchases",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Total price of the purchased product");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantityBought",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Purchases");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Purchases",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Quantity of the product purchased");
        }
    }
}
