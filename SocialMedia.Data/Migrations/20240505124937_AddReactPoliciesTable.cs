using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReactPoliciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Policies");

            migrationBuilder.CreateTable(
            name: "Policies",
            columns: table => new
            {
                Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                PolicyType = table.Column<string>(name: "Policy Type", type: "nvarchar(450)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Policies", x => x.Id);
            });

            migrationBuilder.CreateIndex(
                name: "IX_Policies_Policy Id",
                table: "Policies",
                column: "Policy Type",
                unique: true);

            migrationBuilder.CreateTable(
                name: "ReactPolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(name: "Policy Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReactPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReactPolicies_Policies_Policy Id",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReactPolicies_Policy Id",
                table: "ReactPolicies",
                column: "Policy Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
