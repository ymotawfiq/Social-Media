using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdColumnToPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Post Date",
                table: "Posts",
                newName: "Posted At Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 11, 20, 12, 47, 402, DateTimeKind.Local).AddTicks(8824),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 10, 20, 39, 0, 504, DateTimeKind.Local).AddTicks(3181));

            migrationBuilder.AddColumn<string>(
                name: "User Id",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "b3729f27-b28a-4863-a599-d55c9147fab8");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_User Id",
                table: "Posts",
                column: "User Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_User Id",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "User Id",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "Posted At Date",
                table: "Posts",
                newName: "Post Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 10, 20, 39, 0, 504, DateTimeKind.Local).AddTicks(3181),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 11, 20, 12, 47, 402, DateTimeKind.Local).AddTicks(8824));
        }
    }
}
