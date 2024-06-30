using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
