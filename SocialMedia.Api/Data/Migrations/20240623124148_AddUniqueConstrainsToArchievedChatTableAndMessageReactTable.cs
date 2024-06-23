using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstrainsToArchievedChatTableAndMessageReactTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageReact_Message Id",
                table: "MessageReact");

            migrationBuilder.DropIndex(
                name: "IX_ArchievedChat_User Id",
                table: "ArchievedChat");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReact_Message Id_Reacted User Id",
                table: "MessageReact",
                columns: new[] { "Message Id", "Reacted User Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ArchievedChat_User Id_Chat Id",
                table: "ArchievedChat",
                columns: new[] { "User Id", "Chat Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MessageReact_Message Id_Reacted User Id",
                table: "MessageReact");

            migrationBuilder.DropIndex(
                name: "IX_ArchievedChat_User Id_Chat Id",
                table: "ArchievedChat");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReact_Message Id",
                table: "MessageReact",
                column: "Message Id");

            migrationBuilder.CreateIndex(
                name: "IX_ArchievedChat_User Id",
                table: "ArchievedChat",
                column: "User Id");
        }
    }
}
