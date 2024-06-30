using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivateChatTableAndUpdateChatMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMember_AspNetUsers_Member 2 Id",
                table: "ChatMember");

            migrationBuilder.DropIndex(
                name: "IX_ChatMember_Member 2 Id_Chat Id",
                table: "ChatMember");

            migrationBuilder.DropIndex(
                name: "IX_ChatMember_Member 2 Id_Member 1 Id",
                table: "ChatMember");

            migrationBuilder.DropIndex(
                name: "IX_ChatMember_Member 2 Id_Member 1 Id_Chat Id",
                table: "ChatMember");

            migrationBuilder.DropColumn(
                name: "Member 2 Id",
                table: "ChatMember");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Member 2 Id",
                table: "ChatMember",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMember_Member 2 Id_Chat Id",
                table: "ChatMember",
                columns: new[] { "Member 2 Id", "Chat Id" },
                unique: true,
                filter: "[Member 2 Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMember_Member 2 Id_Member 1 Id",
                table: "ChatMember",
                columns: new[] { "Member 2 Id", "Member 1 Id" },
                unique: true,
                filter: "[Member 2 Id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMember_Member 2 Id_Member 1 Id_Chat Id",
                table: "ChatMember",
                columns: new[] { "Member 2 Id", "Member 1 Id", "Chat Id" },
                unique: true,
                filter: "[Member 2 Id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMember_AspNetUsers_Member 2 Id",
                table: "ChatMember",
                column: "Member 2 Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
