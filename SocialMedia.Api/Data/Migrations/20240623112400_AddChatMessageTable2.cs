using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessageTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_AspNetUsers_UserId",
                table: "ChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_Chat_ChatId",
                table: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_UserId",
                table: "ChatMessage");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ChatMessage");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "ChatMessage",
                newName: "Message Sender Id");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "ChatMessage",
                newName: "Chat Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessage_ChatId",
                table: "ChatMessage",
                newName: "IX_ChatMessage_Chat Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ChatMessage",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "ChatMessage",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Message Sender Id",
                table: "ChatMessage",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_Message Sender Id",
                table: "ChatMessage",
                column: "Message Sender Id");

            migrationBuilder.AddCheckConstraint(
                name: "MessagePhotoCheck",
                table: "ChatMessage",
                sql: "(Photo is NOT null AND Message is null) OR (Photo is null AND Message is NOT null) OR (Message is NOT null AND Photo is NOT null)");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_AspNetUsers_Message Sender Id",
                table: "ChatMessage",
                column: "Message Sender Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_Chat_Chat Id",
                table: "ChatMessage",
                column: "Chat Id",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_AspNetUsers_Message Sender Id",
                table: "ChatMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_Chat_Chat Id",
                table: "ChatMessage");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessage_Message Sender Id",
                table: "ChatMessage");

            migrationBuilder.DropCheckConstraint(
                name: "MessagePhotoCheck",
                table: "ChatMessage");

            migrationBuilder.RenameColumn(
                name: "Message Sender Id",
                table: "ChatMessage",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "Chat Id",
                table: "ChatMessage",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessage_Chat Id",
                table: "ChatMessage",
                newName: "IX_ChatMessage_ChatId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ChatMessage",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "ChatMessage",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "SenderId",
                table: "ChatMessage",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ChatMessage",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessage_UserId",
                table: "ChatMessage",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_AspNetUsers_UserId",
                table: "ChatMessage",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessage_Chat_ChatId",
                table: "ChatMessage",
                column: "ChatId",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
