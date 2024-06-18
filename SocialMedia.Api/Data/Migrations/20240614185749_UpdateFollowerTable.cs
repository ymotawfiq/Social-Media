using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFollowerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Followers_Follower Id_User Id",
                table: "Followers");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 14, 21, 57, 44, 921, DateTimeKind.Local).AddTicks(700),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 14, 10, 49, 37, 982, DateTimeKind.Local).AddTicks(638));

            migrationBuilder.AlterColumn<string>(
                name: "Follower Id",
                table: "Followers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 14, 10, 49, 37, 982, DateTimeKind.Local).AddTicks(638),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 14, 21, 57, 44, 921, DateTimeKind.Local).AddTicks(700));

            migrationBuilder.AlterColumn<string>(
                name: "Follower Id",
                table: "Followers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Followers_Follower Id_User Id",
                table: "Followers",
                columns: new[] { "Follower Id", "User Id" },
                unique: true);
        }
    }
}
