using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConvertFriendIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friends_User Id",
                table: "Friends");

            migrationBuilder.AlterColumn<string>(
                name: "Friend Id",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.DropPrimaryKey(
            name: "PK_Friends",
            table: "Friends");


            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Friends",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddPrimaryKey(
            name: "PK_Friends",
            table: "Friends",
            column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_User Id_Friend Id",
                table: "Friends",
                columns: new[] { "User Id", "Friend Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Friends_User Id_Friend Id",
                table: "Friends");

            migrationBuilder.AlterColumn<string>(
                name: "Friend Id",
                table: "Friends",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Friends",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Friends_User Id",
                table: "Friends",
                column: "User Id");
        }
    }
}
