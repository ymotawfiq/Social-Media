using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountPolicyOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPolicies_Policies_Policy Id",
                table: "AccountPolicies");

            migrationBuilder.AddColumn<string>(
                name: "User Account Policy Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "83939b43-785c-450d-821f-fb3d686c6823");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_User Account Policy Id",
                table: "AspNetUsers",
                column: "User Account Policy Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPolicies_Policies_Policy Id",
                table: "AccountPolicies",
                column: "Policy Id",
                principalTable: "Policies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountPolicies_User Account Policy Id",
                table: "AspNetUsers",
                column: "User Account Policy Id",
                principalTable: "AccountPolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPolicies_Policies_Policy Id",
                table: "AccountPolicies");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountPolicies_User Account Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_User Account Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "User Account Policy Id",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPolicies_Policies_Policy Id",
                table: "AccountPolicies",
                column: "Policy Id",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
