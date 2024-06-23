using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreRestrictsToPrivateChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 1 Id",
                table: "PrivateChat");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 2 Id",
                table: "PrivateChat");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChat_Chat_Chat Id",
                table: "PrivateChat");

            migrationBuilder.DropIndex(
                name: "IX_PrivateChat_User 2 Id",
                table: "PrivateChat");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChat_User 1 Id_Chat Id",
                table: "PrivateChat",
                columns: new[] { "User 1 Id", "Chat Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChat_User 2 Id_Chat Id",
                table: "PrivateChat",
                columns: new[] { "User 2 Id", "Chat Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 1 Id",
                table: "PrivateChat",
                column: "User 1 Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 2 Id",
                table: "PrivateChat",
                column: "User 2 Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChat_Chat_Chat Id",
                table: "PrivateChat",
                column: "Chat Id",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 1 Id",
                table: "PrivateChat");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 2 Id",
                table: "PrivateChat");

            migrationBuilder.DropForeignKey(
                name: "FK_PrivateChat_Chat_Chat Id",
                table: "PrivateChat");

            migrationBuilder.DropIndex(
                name: "IX_PrivateChat_User 1 Id_Chat Id",
                table: "PrivateChat");

            migrationBuilder.DropIndex(
                name: "IX_PrivateChat_User 2 Id_Chat Id",
                table: "PrivateChat");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChat_User 2 Id",
                table: "PrivateChat",
                column: "User 2 Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 1 Id",
                table: "PrivateChat",
                column: "User 1 Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChat_AspNetUsers_User 2 Id",
                table: "PrivateChat",
                column: "User 2 Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PrivateChat_Chat_Chat Id",
                table: "PrivateChat",
                column: "Chat Id",
                principalTable: "Chat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
