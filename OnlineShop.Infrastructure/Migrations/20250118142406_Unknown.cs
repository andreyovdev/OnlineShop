using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineShop.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Unknown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_UserProfiles_UserProfileId",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserProfileId",
                table: "Addresses");

            migrationBuilder.AddColumn<Guid>(
                name: "UserProfileId1",
                table: "Addresses",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AddressId",
                table: "UserProfiles",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserProfileId1",
                table: "Addresses",
                column: "UserProfileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_UserProfiles_UserProfileId1",
                table: "Addresses",
                column: "UserProfileId1",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Addresses_AddressId",
                table: "UserProfiles",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_UserProfiles_UserProfileId1",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Addresses_AddressId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_AddressId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_UserProfileId1",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "UserProfileId1",
                table: "Addresses");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserProfileId",
                table: "Addresses",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_UserProfiles_UserProfileId",
                table: "Addresses",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
