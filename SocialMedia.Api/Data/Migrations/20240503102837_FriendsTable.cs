using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FriendsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "05433b04-03a1-4bf8-83f0-cb60bea5bb56");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2128ca7d-1a61-4d79-bf04-c0e62164d986");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4844107e-7923-4fff-9ba8-69145350b06b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6a9d174f-7c14-44b0-a1e8-53d7b92147b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5360e62-eac4-4476-b143-963e0ab7eb17");

            migrationBuilder.CreateTable(
                name: "Friends",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FriendId = table.Column<string>(name: "Friend Id", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friends", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Friends_AspNetUsers_UserId",
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
                    { "2d24384e-573a-4bd6-8b38-02ff185e9e01", "5", "GroupMember", "GroupMember" },
                    { "3ba3a075-4a04-4292-9a33-afcc37be7495", "4", "Moderator", "Moderator" },
                    { "44ab0232-82ba-49a8-a3af-770e02fc499a", "1", "Admin", "Admin" },
                    { "b7dde6b9-c960-4517-b5fe-b37fdae4d78c", "3", "Owner", "Owner" },
                    { "c956935b-afe8-4da3-8e90-bc417880bab5", "2", "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Friends");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2d24384e-573a-4bd6-8b38-02ff185e9e01");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ba3a075-4a04-4292-9a33-afcc37be7495");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44ab0232-82ba-49a8-a3af-770e02fc499a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b7dde6b9-c960-4517-b5fe-b37fdae4d78c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c956935b-afe8-4da3-8e90-bc417880bab5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "05433b04-03a1-4bf8-83f0-cb60bea5bb56", "5", "GroupMember", "GroupMember" },
                    { "2128ca7d-1a61-4d79-bf04-c0e62164d986", "2", "User", "User" },
                    { "4844107e-7923-4fff-9ba8-69145350b06b", "3", "Owner", "Owner" },
                    { "6a9d174f-7c14-44b0-a1e8-53d7b92147b5", "4", "Moderator", "Moderator" },
                    { "c5360e62-eac4-4476-b143-963e0ab7eb17", "1", "Admin", "Admin" }
                });
        }
    }
}
