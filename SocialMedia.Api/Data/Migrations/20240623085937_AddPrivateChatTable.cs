using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPrivateChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrivateChat",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    User1Id = table.Column<string>(name: "User 1 Id", type: "nvarchar(450)", nullable: false),
                    User2Id = table.Column<string>(name: "User 2 Id", type: "nvarchar(450)", nullable: false),
                    IsBlockedByUser1 = table.Column<bool>(type: "bit", nullable: false),
                    IsBlockedByUser2 = table.Column<bool>(type: "bit", nullable: false),
                    IsBlocked = table.Column<bool>(type: "bit", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ChatId = table.Column<string>(name: "Chat Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivateChat", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivateChat_AspNetUsers_User 1 Id",
                        column: x => x.User1Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrivateChat_AspNetUsers_User 2 Id",
                        column: x => x.User2Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrivateChat_Chat_Chat Id",
                        column: x => x.ChatId,
                        principalTable: "Chat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChat_Chat Id",
                table: "PrivateChat",
                column: "Chat Id");

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChat_User 1 Id_User 2 Id",
                table: "PrivateChat",
                columns: new[] { "User 1 Id", "User 2 Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PrivateChat_User 2 Id",
                table: "PrivateChat",
                column: "User 2 Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrivateChat");
        }
    }
}
