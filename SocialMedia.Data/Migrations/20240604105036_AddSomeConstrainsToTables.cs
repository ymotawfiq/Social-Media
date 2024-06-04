using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSomeConstrainsToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SavedPosts_Post Id",
                table: "SavedPosts");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_Friend Request Person Id",
                table: "FriendRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Image Url",
                table: "PostImages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedPostsFolders_Folder Name_User Id",
                table: "UserSavedPostsFolders",
                columns: new[] { "Folder Name", "User Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_Post Id_Folder Id",
                table: "SavedPosts",
                columns: new[] { "Post Id", "Folder Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_Image Url",
                table: "PostImages",
                column: "Image Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_Friend Request Person Id_User sended friend request Id",
                table: "FriendRequests",
                columns: new[] { "Friend Request Person Id", "User sended friend request Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSavedPostsFolders_Folder Name_User Id",
                table: "UserSavedPostsFolders");

            migrationBuilder.DropIndex(
                name: "IX_SavedPosts_Post Id_Folder Id",
                table: "SavedPosts");

            migrationBuilder.DropIndex(
                name: "IX_PostImages_Image Url",
                table: "PostImages");

            migrationBuilder.DropIndex(
                name: "IX_FriendRequests_Friend Request Person Id_User sended friend request Id",
                table: "FriendRequests");

            migrationBuilder.AlterColumn<string>(
                name: "Image Url",
                table: "PostImages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_Post Id",
                table: "SavedPosts",
                column: "Post Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_Friend Request Person Id",
                table: "FriendRequests",
                column: "Friend Request Person Id",
                unique: true);
        }
    }
}
