using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostCommentReplayTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostReacts_Post Id",
                table: "PostReacts");

            migrationBuilder.CreateTable(
                name: "PostCommentReplay",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostcommentId = table.Column<string>(name: "Post comment Id", type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    Replay = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Replay_Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCommentReplay", x => x.Id);
                    table.CheckConstraint("EncureReplayAndReplayImageNotNull", "(Replay is NOT null AND Replay_Image is NOT null) OR (Replay_Image is null AND Replay is NOT null) OR (Replay is null AND Replay_Image is NOT null)");
                    table.ForeignKey(
                        name: "FK_PostCommentReplay_AspNetUsers_User Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCommentReplay_PostComments_Post comment Id",
                        column: x => x.PostcommentId,
                        principalTable: "PostComments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostReacts_Post Id_User Id",
                table: "PostReacts",
                columns: new[] { "Post Id", "User Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentReplay_Post comment Id",
                table: "PostCommentReplay",
                column: "Post comment Id");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentReplay_User Id",
                table: "PostCommentReplay",
                column: "User Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostCommentReplay");

            migrationBuilder.DropIndex(
                name: "IX_PostReacts_Post Id_User Id",
                table: "PostReacts");

            migrationBuilder.CreateIndex(
                name: "IX_PostReacts_Post Id",
                table: "PostReacts",
                column: "Post Id");
        }
    }
}
