using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropGroupPoliciesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_GroupPolicies_Group Policy Id",
                table: "Groups");

            migrationBuilder.DropTable(
                name: "GroupPolicies");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Policies_Group Policy Id",
                table: "Groups",
                column: "Group Policy Id",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Policies_Group Policy Id",
                table: "Groups");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 14, 10, 9, 40, 18, DateTimeKind.Local).AddTicks(3584),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 14, 10, 49, 37, 982, DateTimeKind.Local).AddTicks(638));

            migrationBuilder.CreateTable(
                name: "GroupPolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(name: "Policy Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupPolicies_Policies_Policy Id",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupPolicies_Policy Id",
                table: "GroupPolicies",
                column: "Policy Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_GroupPolicies_Group Policy Id",
                table: "Groups",
                column: "Group Policy Id",
                principalTable: "GroupPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
