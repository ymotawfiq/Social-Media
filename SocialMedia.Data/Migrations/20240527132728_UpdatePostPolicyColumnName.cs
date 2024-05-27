using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostPolicyColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PostPolicies_PostPolicyId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "PostPolicyId",
                table: "AspNetUsers",
                newName: "Account post Policy Id");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_PostPolicyId",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Account post Policy Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PostPolicies_Account post Policy Id",
                table: "AspNetUsers",
                column: "Account post Policy Id",
                principalTable: "PostPolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PostPolicies_Account post Policy Id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "Account post Policy Id",
                table: "AspNetUsers",
                newName: "PostPolicyId");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Account post Policy Id",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_PostPolicyId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PostPolicies_PostPolicyId",
                table: "AspNetUsers",
                column: "PostPolicyId",
                principalTable: "PostPolicies",
                principalColumn: "Id");
        }
    }
}
