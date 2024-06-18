using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFriendRequestsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7ec740e1-657d-4ae5-92e3-7cc5364a8528");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1e78aa2-2689-44f1-b15c-aefb087c01cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c94f6e30-d1af-4536-aabf-ea1149e7f704");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fc099eed-4c03-4c8e-998d-93ad739eb012");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fff06123-ded3-42be-849f-07acce8f7a59");

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsersendedfriendrequestId = table.Column<string>(name: "User sended friend request Id", type: "nvarchar(450)", nullable: false),
                    FriendRequestPersonId = table.Column<string>(name: "Friend Request Person Id", type: "nvarchar(450)", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_User sended friend request Id",
                        column: x => x.UsersendedfriendrequestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_Friend Request Person Id",
                table: "FriendRequests",
                column: "Friend Request Person Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_User sended friend request Id",
                table: "FriendRequests",
                column: "User sended friend request Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendRequests");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "7ec740e1-657d-4ae5-92e3-7cc5364a8528", "4", "Moderator", "Moderator" },
                    { "a1e78aa2-2689-44f1-b15c-aefb087c01cc", "3", "Owner", "Owner" },
                    { "c94f6e30-d1af-4536-aabf-ea1149e7f704", "5", "GroupMember", "GroupMember" },
                    { "fc099eed-4c03-4c8e-998d-93ad739eb012", "1", "Admin", "Admin" },
                    { "fff06123-ded3-42be-849f-07acce8f7a59", "2", "User", "User" }
                });
        }
    }
}
