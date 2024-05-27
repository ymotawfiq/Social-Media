using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentPolicies_Policies_Policy Id",
                table: "CommentPolicies");

            migrationBuilder.RenameColumn(
                name: "Policy Id",
                table: "CommentPolicies",
                newName: "PolicyId");

            migrationBuilder.RenameIndex(
                name: "IX_CommentPolicies_Policy Id",
                table: "CommentPolicies",
                newName: "IX_CommentPolicies_PolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentPolicies_Policies_PolicyId",
                table: "CommentPolicies",
                column: "PolicyId",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentPolicies_Policies_PolicyId",
                table: "CommentPolicies");

            migrationBuilder.RenameColumn(
                name: "PolicyId",
                table: "CommentPolicies",
                newName: "Policy Id");

            migrationBuilder.RenameIndex(
                name: "IX_CommentPolicies_PolicyId",
                table: "CommentPolicies",
                newName: "IX_CommentPolicies_Policy Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentPolicies_Policies_Policy Id",
                table: "CommentPolicies",
                column: "Policy Id",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
