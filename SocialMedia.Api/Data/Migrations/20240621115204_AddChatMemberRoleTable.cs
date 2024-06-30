using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChatMemberRoleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChatMemberRole",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChatMemberId = table.Column<string>(name: "Chat Member Id", type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(name: "Role Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMemberRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMemberRole_ChatMember_Chat Member Id",
                        column: x => x.ChatMemberId,
                        principalTable: "ChatMember",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatMemberRole_Role_Role Id",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMemberRole_Chat Member Id_Role Id",
                table: "ChatMemberRole",
                columns: new[] { "Chat Member Id", "Role Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMemberRole_Role Id",
                table: "ChatMemberRole",
                column: "Role Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMemberRole");
        }
    }
}
