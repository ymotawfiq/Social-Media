using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostCommentReplayToReplay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Post Comment Replay Id",
                table: "PostCommentReplay",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentReplay_Post Comment Replay Id",
                table: "PostCommentReplay",
                column: "Post Comment Replay Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCommentReplay_PostCommentReplay_Post Comment Replay Id",
                table: "PostCommentReplay",
                column: "Post Comment Replay Id",
                principalTable: "PostCommentReplay",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCommentReplay_PostCommentReplay_Post Comment Replay Id",
                table: "PostCommentReplay");

            migrationBuilder.DropIndex(
                name: "IX_PostCommentReplay_Post Comment Replay Id",
                table: "PostCommentReplay");

            migrationBuilder.DropColumn(
                name: "Post Comment Replay Id",
                table: "PostCommentReplay");
        }
    }
}
