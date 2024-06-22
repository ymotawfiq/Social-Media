using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropOldChatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReacts_AspNetUsers_Reacted User Id",
                table: "MessageReacts");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReacts_ChatMessages_Message Id",
                table: "MessageReacts");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageReacts_Reacts_React Id",
                table: "MessageReacts");

            migrationBuilder.DropTable(
                name: "ArchievedChats");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatRequests");

            migrationBuilder.DropTable(
                name: "UserChats");

            migrationBuilder.DropIndex(
                name: "IX_MessageReacts_Message Id",
                table: "MessageReacts");

            migrationBuilder.DropIndex(
                name: "IX_MessageReacts_Reacted User Id",
                table: "MessageReacts");

            migrationBuilder.DropTable(
                name: "MessageReacts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageReacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReactId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    MessageId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReactedUserId = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReacts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_MessageReacts_Reacts_ReactId",
                        column: x => x.ReactId,
                        principalTable: "Reacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReacts_ReactId",
                table: "MessageReacts",
                column: "ReactId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReacts_UserId",
                table: "MessageReacts",
                column: "UserId");
        }
    }
}
