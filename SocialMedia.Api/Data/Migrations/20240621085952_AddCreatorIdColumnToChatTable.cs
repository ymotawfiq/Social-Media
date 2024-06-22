using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCreatorIdColumnToChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Creator Id",
                table: "Chat",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Chat_Creator Id",
                table: "Chat",
                column: "Creator Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Chat_AspNetUsers_Creator Id",
                table: "Chat",
                column: "Creator Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chat_AspNetUsers_Creator Id",
                table: "Chat");

            migrationBuilder.DropIndex(
                name: "IX_Chat_Creator Id",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "Creator Id",
                table: "Chat");
        }
    }
}
