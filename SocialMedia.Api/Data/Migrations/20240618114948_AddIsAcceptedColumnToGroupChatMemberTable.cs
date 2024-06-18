using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsAcceptedColumnToGroupChatMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMembers_GroupChats_Group Chat Id",
                table: "GroupChatMembers");

            migrationBuilder.AddColumn<bool>(
                name: "Is Join Request Accepted",
                table: "GroupChatMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMembers_GroupChats_Group Chat Id",
                table: "GroupChatMembers",
                column: "Group Chat Id",
                principalTable: "GroupChats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupChatMembers_GroupChats_Group Chat Id",
                table: "GroupChatMembers");

            migrationBuilder.DropColumn(
                name: "Is Join Request Accepted",
                table: "GroupChatMembers");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupChatMembers_GroupChats_Group Chat Id",
                table: "GroupChatMembers",
                column: "Group Chat Id",
                principalTable: "GroupChats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
