using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class Test6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropTable(
                name: "PostCommentReplay");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 16, 0, 31, 2, 699, DateTimeKind.Local).AddTicks(4257),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 15, 14, 10, 53, 225, DateTimeKind.Local).AddTicks(7108));

            migrationBuilder.AddColumn<string>(
                name: "BaseCommentId",
                table: "PostComments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommentId",
                table: "PostComments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_BaseCommentId",
                table: "PostComments",
                column: "BaseCommentId");


            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_PostComments_BaseCommentId",
                table: "PostComments",
                column: "BaseCommentId",
                principalTable: "PostComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages");

            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_PostComments_BaseCommentId",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_BaseCommentId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "BaseCommentId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "PostComments");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 15, 14, 10, 53, 225, DateTimeKind.Local).AddTicks(7108),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 16, 0, 31, 2, 699, DateTimeKind.Local).AddTicks(4257));

            migrationBuilder.CreateTable(
                name: "PostCommentReplay",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostcommentId = table.Column<string>(name: "Post comment Id", type: "nvarchar(450)", nullable: false),
                    PostCommentReplayId = table.Column<string>(name: "Post Comment Replay Id", type: "nvarchar(450)", nullable: true),
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
                        name: "FK_PostCommentReplay_PostCommentReplay_Post Comment Replay Id",
                        column: x => x.PostCommentReplayId,
                        principalTable: "PostCommentReplay",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PostCommentReplay_PostComments_Post comment Id",
                        column: x => x.PostcommentId,
                        principalTable: "PostComments",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentReplay_Post comment Id",
                table: "PostCommentReplay",
                column: "Post comment Id");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentReplay_Post Comment Replay Id",
                table: "PostCommentReplay",
                column: "Post Comment Replay Id");

            migrationBuilder.CreateIndex(
                name: "IX_PostCommentReplay_User Id",
                table: "PostCommentReplay",
                column: "User Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
