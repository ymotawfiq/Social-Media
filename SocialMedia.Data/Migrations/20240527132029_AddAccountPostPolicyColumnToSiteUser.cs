using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountPostPolicyColumnToSiteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PostPolicyId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PostPolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(name: "Policy Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostPolicies_Policies_Policy Id",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_PostPolicyId",
                table: "AspNetUsers",
                column: "PostPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_PostPolicies_Policy Id",
                table: "PostPolicies",
                column: "Policy Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_PostPolicies_PostPolicyId",
                table: "AspNetUsers",
                column: "PostPolicyId",
                principalTable: "PostPolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_PostPolicies_PostPolicyId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "PostPolicies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_PostPolicyId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PostPolicyId",
                table: "AspNetUsers");
        }
    }
}
