using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSentAtTimeToSarehneMessageTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 10, 20, 39, 0, 504, DateTimeKind.Local).AddTicks(3181));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SentAt",
                table: "SarehneMessages");
        }
    }
}
