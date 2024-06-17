using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMessagesTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {


            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessage_UserChats_ChatId",
                table: "ChatMessage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessage",
                table: "ChatMessage");


            migrationBuilder.RenameTable(
                name: "ChatMessage",
                newName: "ChatMessages");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "ChatMessages",
                newName: "Sender Id");

            migrationBuilder.RenameColumn(
                name: "ChatId",
                table: "ChatMessages",
                newName: "Chat Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessage_ChatId",
                table: "ChatMessages",
                newName: "IX_ChatMessages_Chat Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 17, 9, 30, 18, 347, DateTimeKind.Local).AddTicks(5614),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 17, 9, 20, 15, 382, DateTimeKind.Local).AddTicks(8438));

            migrationBuilder.AlterColumn<string>(
                name: "Sender Id",
                table: "ChatMessages",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_Sender Id",
                table: "ChatMessages",
                column: "Sender Id");

            //migrationBuilder.AddCheckConstraint(
            //    name: "CheckPhotoAndImageNotPothNull",
            //    table: "ChatMessages",
            //    sql: "(Photo is NOT null AND Message is null) OR (Photo is null AND Message is NOT null)");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_AspNetUsers_Sender Id",
                table: "ChatMessages",
                column: "Sender Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_UserChats_Chat Id",
                table: "ChatMessages",
                column: "Chat Id",
                principalTable: "UserChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_AspNetUsers_Sender Id",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_UserChats_Chat Id",
                table: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChatMessages",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_Sender Id",
                table: "ChatMessages");

            migrationBuilder.DropCheckConstraint(
                name: "CheckPhotoAndImageNotPothNull",
                table: "ChatMessages");

            migrationBuilder.RenameTable(
                name: "ChatMessages",
                newName: "ChatMessage");

            migrationBuilder.RenameColumn(
                name: "Sender Id",
                table: "ChatMessage",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "Chat Id",
                table: "ChatMessage",
                newName: "ChatId");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMessages_Chat Id",
                table: "ChatMessage",
                newName: "IX_ChatMessage_ChatId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 17, 9, 20, 15, 382, DateTimeKind.Local).AddTicks(8438),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 17, 9, 30, 18, 347, DateTimeKind.Local).AddTicks(5614));

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChatMessage",
                table: "ChatMessage",
                column: "Id");

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
                name: "FK_ChatMessage_UserChats_ChatId",
                table: "ChatMessage",
                column: "ChatId",
                principalTable: "UserChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
