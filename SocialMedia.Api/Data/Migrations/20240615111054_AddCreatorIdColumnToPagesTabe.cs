using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorIdColumnToPagesTabe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 15, 14, 10, 53, 225, DateTimeKind.Local).AddTicks(7108),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 15, 14, 7, 59, 462, DateTimeKind.Local).AddTicks(8609));

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "Pages",
                type: "nvarchar(450)",
                nullable: false
               );

            migrationBuilder.CreateIndex(
                name: "IX_Pages_CreatorId",
                table: "Pages",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages");

            migrationBuilder.DropIndex(
                name: "IX_Pages_CreatorId",
                table: "Pages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 15, 14, 7, 59, 462, DateTimeKind.Local).AddTicks(8609),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 15, 14, 10, 53, 225, DateTimeKind.Local).AddTicks(7108));

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
