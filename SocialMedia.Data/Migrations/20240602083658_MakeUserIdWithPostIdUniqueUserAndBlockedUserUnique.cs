using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeUserIdWithPostIdUniqueUserAndBlockedUserUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPosts_User Id",
                table: "UserPosts");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_User Id",
                table: "Blocks");

            migrationBuilder.AlterColumn<string>(
                name: "Blocked User Id",
                table: "Blocks",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_User Id_Post Id",
                table: "UserPosts",
                columns: new[] { "User Id", "Post Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_User Id_Blocked User Id",
                table: "Blocks",
                columns: new[] { "User Id", "Blocked User Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserPosts_User Id_Post Id",
                table: "UserPosts");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_User Id_Blocked User Id",
                table: "Blocks");

            migrationBuilder.AlterColumn<string>(
                name: "Blocked User Id",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_User Id",
                table: "UserPosts",
                column: "User Id");

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_User Id",
                table: "Blocks",
                column: "User Id");
        }
    }
}
