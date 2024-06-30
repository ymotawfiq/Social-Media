using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePolicyIdFromPostsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropIndex(
            //    name: "IX_Posts_PolicyId",
            //    table: "Posts");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Posts_Policies_PolicyId",
            //    table: "Posts");

            //migrationBuilder.DropColumn(
            //    name: "PolicyId",
            //    table: "Posts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 12, 23, 4, 58, 175, DateTimeKind.Local).AddTicks(3105),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 12, 23, 24, 58, 818, DateTimeKind.Local).AddTicks(2345));
        }
    }
}
