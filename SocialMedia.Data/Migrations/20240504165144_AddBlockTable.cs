using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlockTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36de1b49-a0e4-4cdc-a554-daf2c69bb73f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44b45a9a-3597-45b4-8f38-14a57cd90a2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66daa28b-0259-44f2-a8ad-029331f7881f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7db8f866-3c78-4dec-9e97-ea46c8590949");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aad83349-110e-42ca-867a-cc5aef0305d4");

            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    BlockedUserId = table.Column<string>(name: "Blocked User Id", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blocks_AspNetUsers_User Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6d2e84fd-78b0-428a-a7d5-5cb3c6f9c193", "4", "Moderator", "Moderator" },
                    { "6e6882bb-06f8-4904-8a59-3fb4e1a73bc5", "5", "GroupMember", "GroupMember" },
                    { "6fe62bc1-9ff4-4aa6-afa5-f02493db4cd6", "2", "User", "User" },
                    { "83f68313-3667-497a-9c68-99339a205fc1", "1", "Admin", "Admin" },
                    { "f2b9c601-8d8b-4dfa-8e4e-30233e156a7d", "3", "Owner", "Owner" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_User Id",
                table: "Blocks",
                column: "User Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blocks");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6d2e84fd-78b0-428a-a7d5-5cb3c6f9c193");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6e6882bb-06f8-4904-8a59-3fb4e1a73bc5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6fe62bc1-9ff4-4aa6-afa5-f02493db4cd6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "83f68313-3667-497a-9c68-99339a205fc1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2b9c601-8d8b-4dfa-8e4e-30233e156a7d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "36de1b49-a0e4-4cdc-a554-daf2c69bb73f", "5", "GroupMember", "GroupMember" },
                    { "44b45a9a-3597-45b4-8f38-14a57cd90a2c", "2", "User", "User" },
                    { "66daa28b-0259-44f2-a8ad-029331f7881f", "1", "Admin", "Admin" },
                    { "7db8f866-3c78-4dec-9e97-ea46c8590949", "3", "Owner", "Owner" },
                    { "aad83349-110e-42ca-867a-cc5aef0305d4", "4", "Moderator", "Moderator" }
                });
        }
    }
}
