using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupChatTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReacts_ChatMessages_Message Id",
                table: "MessageReacts");

            migrationBuilder.CreateTable(
                name: "GroupChats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(name: "Group Name", type: "nvarchar(max)", nullable: false),
                    CreatedByUserId = table.Column<string>(name: "Created By User Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChats_AspNetUsers_Created By User Id",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChats_Created By User Id",
                table: "GroupChats",
                column: "Created By User Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReacts_ChatMessages_Message Id",
                table: "MessageReacts",
                column: "Message Id",
                principalTable: "ChatMessages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageReacts_ChatMessages_Message Id",
                table: "MessageReacts");

            migrationBuilder.DropTable(
                name: "GroupChats");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageReacts_ChatMessages_Message Id",
                table: "MessageReacts",
                column: "Message Id",
                principalTable: "ChatMessages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
