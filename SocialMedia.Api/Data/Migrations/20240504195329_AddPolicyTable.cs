using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPolicyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "Policies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PolicyType = table.Column<string>(name: "Policy Type", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Policies", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2dcc92bb-ac49-4b72-b429-bccbb6e5a8bc", "4", "Moderator", "Moderator" },
                    { "c4380211-7b12-438c-86ad-faed304fc1f2", "1", "Admin", "Admin" },
                    { "cf9fe55c-1df9-4e86-aac7-1b657f2799fc", "3", "Owner", "Owner" },
                    { "efe0eaf9-27ba-4865-8617-42d16a5beb66", "5", "GroupMember", "GroupMember" },
                    { "f6a4ab99-dd2c-4922-aa17-17a922977a5a", "2", "User", "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Policies_Policy Type",
                table: "Policies",
                column: "Policy Type",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2dcc92bb-ac49-4b72-b429-bccbb6e5a8bc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c4380211-7b12-438c-86ad-faed304fc1f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cf9fe55c-1df9-4e86-aac7-1b657f2799fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "efe0eaf9-27ba-4865-8617-42d16a5beb66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f6a4ab99-dd2c-4922-aa17-17a922977a5a");

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
        }
    }
}
