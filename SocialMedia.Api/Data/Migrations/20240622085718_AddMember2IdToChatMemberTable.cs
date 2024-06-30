using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMember2IdToChatMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMember_AspNetUsers_Member Id",
                table: "ChatMember");

            migrationBuilder.RenameColumn(
                name: "Member Id",
                table: "ChatMember",
                newName: "Member 1 Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMember_Member Id_Chat Id",
                table: "ChatMember",
                newName: "IX_ChatMember_Member 1 Id_Chat Id");

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
                name: "FK_ChatMember_AspNetUsers_Member 1 Id",
                table: "ChatMember",
                column: "Member 1 Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMember_AspNetUsers_Member 2 Id",
                table: "ChatMember",
                column: "Member 2 Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMember_AspNetUsers_Member 1 Id",
                table: "ChatMember");

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

            migrationBuilder.RenameColumn(
                name: "Member 1 Id",
                table: "ChatMember",
                newName: "Member Id");

            migrationBuilder.RenameIndex(
                name: "IX_ChatMember_Member 1 Id_Chat Id",
                table: "ChatMember",
                newName: "IX_ChatMember_Member Id_Chat Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMember_AspNetUsers_Member Id",
                table: "ChatMember",
                column: "Member Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
