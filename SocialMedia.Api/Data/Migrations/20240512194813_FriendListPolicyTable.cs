using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FriendListPolicyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendListPolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(name: "Policy Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendListPolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FriendListPolicies_AspNetUsers_User Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendListPolicies_Policies_Policy Id",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendListPolicies_Policy Id",
                table: "FriendListPolicies",
                column: "Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendListPolicies_User Id",
                table: "FriendListPolicies",
                column: "User Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendListPolicies");
        }
    }
}
