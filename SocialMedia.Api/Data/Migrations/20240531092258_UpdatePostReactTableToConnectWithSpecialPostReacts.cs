using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostReactTableToConnectWithSpecialPostReacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReacts_Reacts_React Id",
                table: "PostReacts");

            migrationBuilder.RenameColumn(
                name: "React Id",
                table: "PostReacts",
                newName: "Post React Id");

            migrationBuilder.RenameIndex(
                name: "IX_PostReacts_React Id",
                table: "PostReacts",
                newName: "IX_PostReacts_Post React Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReacts_SpecialPostReacts_Post React Id",
                table: "PostReacts",
                column: "Post React Id",
                principalTable: "SpecialPostReacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReacts_SpecialPostReacts_Post React Id",
                table: "PostReacts");

            migrationBuilder.RenameColumn(
                name: "Post React Id",
                table: "PostReacts",
                newName: "React Id");

            migrationBuilder.RenameIndex(
                name: "IX_PostReacts_Post React Id",
                table: "PostReacts",
                newName: "IX_PostReacts_React Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PostReacts_Reacts_React Id",
                table: "PostReacts",
                column: "React Id",
                principalTable: "Reacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
