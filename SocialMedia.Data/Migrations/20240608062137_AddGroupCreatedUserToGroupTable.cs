using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupCreatedUserToGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Group Creator Id",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Group Creator Id",
                table: "Groups",
                column: "Group Creator Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_AspNetUsers_Group Creator Id",
                table: "Groups",
                column: "Group Creator Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_AspNetUsers_Group Creator Id",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Group Creator Id",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Group Creator Id",
                table: "Groups");
        }
    }
}
