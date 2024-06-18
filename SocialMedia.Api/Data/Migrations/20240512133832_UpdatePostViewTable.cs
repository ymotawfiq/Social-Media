using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostViewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostViews_AspNetUsers_User Id",
                table: "PostViews");

            migrationBuilder.DropIndex(
                name: "IX_PostViews_User Id",
                table: "PostViews");

            migrationBuilder.DropColumn(
                name: "User Id",
                table: "PostViews");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "User Id",
                table: "PostViews",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_User Id",
                table: "PostViews",
                column: "User Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostViews_AspNetUsers_User Id",
                table: "PostViews",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
