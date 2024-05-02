using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReactTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24e485de-3e01-4272-acc5-5cea03f089cb");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36f46ec0-836e-4790-b036-5e8c48d0bae2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "65001ed3-ebb2-4e71-bad3-cb59b70babfc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ddb5d2b2-f1bd-4b8d-b309-745e29055878");

            migrationBuilder.CreateTable(
                name: "Reacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactValue = table.Column<string>(name: "React Value", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reacts", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reacts");

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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24e485de-3e01-4272-acc5-5cea03f089cb", "3", "Owner", "Owner" },
                    { "36f46ec0-836e-4790-b036-5e8c48d0bae2", "1", "Admin", "Admin" },
                    { "65001ed3-ebb2-4e71-bad3-cb59b70babfc", "4", "moderator", "moderator" },
                    { "ddb5d2b2-f1bd-4b8d-b309-745e29055878", "2", "User", "User" }
                });
        }
    }
}
