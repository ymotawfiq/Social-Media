using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAcceptedColumnToChatRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRequests_AspNetUsers_UserWhoSentRequestId",
                table: "ChatRequests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 17, 8, 24, 24, 317, DateTimeKind.Local).AddTicks(3255),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 16, 21, 37, 56, 846, DateTimeKind.Local).AddTicks(5722));

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "ChatRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRequests_AspNetUsers_UserWhoSentRequestId",
                table: "ChatRequests",
                column: "UserWhoSentRequestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatRequests_AspNetUsers_UserWhoSentRequestId",
                table: "ChatRequests");

            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "ChatRequests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 16, 21, 37, 56, 846, DateTimeKind.Local).AddTicks(5722),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 17, 8, 24, 24, 317, DateTimeKind.Local).AddTicks(3255));

            migrationBuilder.AddForeignKey(
                name: "FK_ChatRequests_AspNetUsers_UserWhoSentRequestId",
                table: "ChatRequests",
                column: "UserWhoSentRequestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
