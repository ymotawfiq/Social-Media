using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageReactTable2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MessageReact",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MessageId = table.Column<string>(name: "Message Id", type: "nvarchar(450)", nullable: false),
                    ReactedUserId = table.Column<string>(name: "Reacted User Id", type: "nvarchar(450)", nullable: false),
                    ReactId = table.Column<string>(name: "React Id", type: "nvarchar(450)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageReact", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageReact_AspNetUsers_Reacted User Id",
                        column: x => x.ReactedUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReact_ChatMessage_Message Id",
                        column: x => x.MessageId,
                        principalTable: "ChatMessage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MessageReact_Reacts_React Id",
                        column: x => x.ReactId,
                        principalTable: "Reacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MessageReact_Message Id",
                table: "MessageReact",
                column: "Message Id");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReact_React Id",
                table: "MessageReact",
                column: "React Id");

            migrationBuilder.CreateIndex(
                name: "IX_MessageReact_Reacted User Id_Message Id",
                table: "MessageReact",
                columns: new[] { "Reacted User Id", "Message Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageReact");
        }
    }
}
