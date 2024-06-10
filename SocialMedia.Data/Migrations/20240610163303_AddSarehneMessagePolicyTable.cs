using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSarehneMessagePolicyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Message Policy Id",
                table: "SarehneMessages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "SarehneMessagePolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(name: "Policy Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SarehneMessagePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SarehneMessagePolicies_Policies_Policy Id",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SarehneMessages_Message Policy Id",
                table: "SarehneMessages",
                column: "Message Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_SarehneMessagePolicies_Policy Id",
                table: "SarehneMessagePolicies",
                column: "Policy Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SarehneMessages_SarehneMessagePolicies_Message Policy Id",
                table: "SarehneMessages",
                column: "Message Policy Id",
                principalTable: "SarehneMessagePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SarehneMessages_SarehneMessagePolicies_Message Policy Id",
                table: "SarehneMessages");

            migrationBuilder.DropTable(
                name: "SarehneMessagePolicies");

            migrationBuilder.DropIndex(
                name: "IX_SarehneMessages_Message Policy Id",
                table: "SarehneMessages");

            migrationBuilder.DropColumn(
                name: "Message Policy Id",
                table: "SarehneMessages");
        }
    }
}
