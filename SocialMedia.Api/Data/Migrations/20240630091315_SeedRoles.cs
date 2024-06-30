using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "97d644a8-eff5-4700-aef8-8b4f29b8aea9", "2", "USER", "USER" },
                    { "facb6f32-ae7f-4cf9-a386-5463487344f0", "1", "ADMIN", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97d644a8-eff5-4700-aef8-8b4f29b8aea9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "facb6f32-ae7f-4cf9-a386-5463487344f0");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16e475b7-b8ad-4352-8366-a08f7f397111", "2", "USER", "USER" },
                    { "cbc365f5-8533-46f5-a330-5195296a2c0b", "1", "ADMIN", "ADMIN" }
                });
        }
    }
}
