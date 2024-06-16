using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChatRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 16, 21, 37, 56, 846, DateTimeKind.Local).AddTicks(5722),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 16, 20, 36, 32, 307, DateTimeKind.Local).AddTicks(2524));

            migrationBuilder.CreateTable(
                name: "ChatRequests",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserWhoSentRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserWhoReceivedRequestId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatRequests_AspNetUsers_UserWhoReceivedRequestId",
                        column: x => x.UserWhoReceivedRequestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChatRequests_AspNetUsers_UserWhoSentRequestId",
                        column: x => x.UserWhoSentRequestId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChatRequests_UserWhoReceivedRequestId_UserWhoSentRequestId",
                table: "ChatRequests",
                columns: new[] { "UserWhoReceivedRequestId", "UserWhoSentRequestId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRequests_UserWhoSentRequestId",
                table: "ChatRequests",
                column: "UserWhoSentRequestId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChats_AspNetUsers_User2Id",
                table: "UserChats");

            migrationBuilder.DropTable(
                name: "ChatRequests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 16, 20, 36, 32, 307, DateTimeKind.Local).AddTicks(2524),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 16, 21, 37, 56, 846, DateTimeKind.Local).AddTicks(5722));

            migrationBuilder.AddForeignKey(
                name: "FK_UserChats_AspNetUsers_User2Id",
                table: "UserChats",
                column: "User2Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
