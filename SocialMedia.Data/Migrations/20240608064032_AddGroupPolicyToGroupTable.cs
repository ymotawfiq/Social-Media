using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupPolicyToGroupTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Group Policy Id",
                table: "Groups",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_Group Policy Id",
                table: "Groups",
                column: "Group Policy Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupPolicies_Group Policy Id",
                table: "Groups",
                column: "Group Policy Id",
                principalTable: "GroupPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupPolicies_Group Policy Id",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_Group Policy Id",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "Group Policy Id",
                table: "Groups");
        }
    }
}
