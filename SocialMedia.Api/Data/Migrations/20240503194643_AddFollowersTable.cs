using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "480b66d0-16c1-4051-9b63-2eae6a750910");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "57bb5f24-aba6-4157-a0c6-e388ceffb149");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "74b417ab-78c2-47a2-ab96-758ac66a2dea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "797a0b34-2159-4aea-b997-33bf6a8d6fa0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a45b6188-0f4e-4ac6-adb3-d66a9278cae5");

            migrationBuilder.CreateTable(
                name: "Followers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    FollowerId = table.Column<string>(name: "Follower Id", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Followers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Followers_AspNetUsers_User Id",
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
                    { "5312f69a-cc3b-448a-b0de-db27576f2d97", "1", "Admin", "Admin" },
                    { "7932404a-0a05-442b-8d9c-02687251d9f2", "5", "GroupMember", "GroupMember" },
                    { "a3f20d50-137e-4b85-b7e4-86f68ba1a8af", "2", "User", "User" },
                    { "c170ab54-4fc3-4c38-8dc7-d065528b6113", "3", "Owner", "Owner" },
                    { "c178cfe6-319f-4b13-a31a-1571e052f839", "4", "Moderator", "Moderator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Followers_User Id",
                table: "Followers",
                column: "User Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Followers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5312f69a-cc3b-448a-b0de-db27576f2d97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7932404a-0a05-442b-8d9c-02687251d9f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3f20d50-137e-4b85-b7e4-86f68ba1a8af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c170ab54-4fc3-4c38-8dc7-d065528b6113");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c178cfe6-319f-4b13-a31a-1571e052f839");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "480b66d0-16c1-4051-9b63-2eae6a750910", "1", "Admin", "Admin" },
                    { "57bb5f24-aba6-4157-a0c6-e388ceffb149", "3", "Owner", "Owner" },
                    { "74b417ab-78c2-47a2-ab96-758ac66a2dea", "2", "User", "User" },
                    { "797a0b34-2159-4aea-b997-33bf6a8d6fa0", "5", "GroupMember", "GroupMember" },
                    { "a45b6188-0f4e-4ac6-adb3-d66a9278cae5", "4", "Moderator", "Moderator" }
                });
        }
    }
}
