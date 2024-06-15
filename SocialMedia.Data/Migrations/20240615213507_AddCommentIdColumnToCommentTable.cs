using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentIdColumnToCommentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_PostComments_BaseCommentId",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_BaseCommentId",
                table: "PostComments");

            migrationBuilder.DropColumn(
                name: "BaseCommentId",
                table: "PostComments");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "PostComments",
                newName: "Base Comment Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 16, 0, 35, 6, 629, DateTimeKind.Local).AddTicks(4064),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 16, 0, 31, 2, 699, DateTimeKind.Local).AddTicks(4257));

            migrationBuilder.AlterColumn<string>(
                name: "Base Comment Id",
                table: "PostComments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_Base Comment Id",
                table: "PostComments",
                column: "Base Comment Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_PostComments_Base Comment Id",
                table: "PostComments",
                column: "Base Comment Id",
                principalTable: "PostComments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_PostComments_Base Comment Id",
                table: "PostComments");

            migrationBuilder.DropIndex(
                name: "IX_PostComments_Base Comment Id",
                table: "PostComments");

            migrationBuilder.RenameColumn(
                name: "Base Comment Id",
                table: "PostComments",
                newName: "CommentId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 16, 0, 31, 2, 699, DateTimeKind.Local).AddTicks(4257),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 16, 0, 35, 6, 629, DateTimeKind.Local).AddTicks(4064));

            migrationBuilder.AlterColumn<string>(
                name: "CommentId",
                table: "PostComments",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BaseCommentId",
                table: "PostComments",
                type: "nvarchar(450)",
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
    }
}
