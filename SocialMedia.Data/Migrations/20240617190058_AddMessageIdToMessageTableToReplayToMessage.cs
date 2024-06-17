using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageIdToMessageTableToReplayToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchievedChats_UserChats_Chat Id",
                table: "ArchievedChats");

            migrationBuilder.AddColumn<string>(
                name: "MessageId",
                table: "ChatMessages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_MessageId",
                table: "ChatMessages",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchievedChats_UserChats_Chat Id",
                table: "ArchievedChats",
                column: "Chat Id",
                principalTable: "UserChats",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_ChatMessages_MessageId",
                table: "ChatMessages",
                column: "MessageId",
                principalTable: "ChatMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArchievedChats_UserChats_Chat Id",
                table: "ArchievedChats");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_ChatMessages_MessageId",
                table: "ChatMessages");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_MessageId",
                table: "ChatMessages");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "ChatMessages");

            migrationBuilder.AddForeignKey(
                name: "FK_ArchievedChats_UserChats_Chat Id",
                table: "ArchievedChats",
                column: "Chat Id",
                principalTable: "UserChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
