using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class WishlistCartPurchaseAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserProducts");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Name of the user profile",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Name of the user");

            migrationBuilder.AddColumn<Guid>(
                name: "AppUserId",
                table: "UserProfiles",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                comment: "Foreign key to AppUser");

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the user profile"),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the product"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity of the product in the cart")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => new { x.UserProfileId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Carts_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                },
                comment: "Table of products in users carts");

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the user"),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the product"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity of the product purchased"),
                    DatePurchased = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "Date when the product was purchased")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => new { x.UserProfileId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_Purchases_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Purchases_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                },
                comment: "Table of products purchased by users");

            migrationBuilder.CreateTable(
                name: "Wishlists",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the user profile"),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the product")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wishlists", x => new { x.UserProfileId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_Wishlists_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Wishlists_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                },
                comment: "Table of products wishlisted users");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AppUserId",
                table: "UserProfiles",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductId",
                table: "Carts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_ProductId",
                table: "Purchases",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_ProductId",
                table: "Wishlists",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Users_AppUserId",
                table: "UserProfiles",
                column: "AppUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Users_AppUserId",
                table: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_AppUserId",
                table: "UserProfiles");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "UserProfiles");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "UserProfiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Name of the user",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Name of the user profile");

            migrationBuilder.CreateTable(
                name: "UserProducts",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the user"),
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Id of the product")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProducts", x => new { x.UserId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_UserProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UserProducts_UserProfiles_UserId",
                        column: x => x.UserId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                },
                comment: "Table of products and users");

            migrationBuilder.CreateIndex(
                name: "IX_UserProducts_ProductId",
                table: "UserProducts",
                column: "ProductId");
        }
    }
}
