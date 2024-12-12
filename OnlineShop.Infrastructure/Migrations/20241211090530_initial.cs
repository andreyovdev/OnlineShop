using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the product"),
                    Category = table.Column<int>(type: "int", nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "URL of the product image"),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false, comment: "Description of the product"),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Available quantity of the product"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false, comment: "Indicates if the product is deleted")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                },
                comment: "Table of products");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
