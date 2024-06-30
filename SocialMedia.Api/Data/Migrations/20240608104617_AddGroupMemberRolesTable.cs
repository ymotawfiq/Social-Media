using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupMemberRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GroupMemberRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupMemberId = table.Column<string>(name: "Group Member Id", type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(name: "Role Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMemberRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupMemberRoles_GroupMembers_Group Member Id",
                        column: x => x.GroupMemberId,
                        principalTable: "GroupMembers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupMemberRoles_GroupRoles_Role Id",
                        column: x => x.RoleId,
                        principalTable: "GroupRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberRoles_Group Member Id_Role Id",
                table: "GroupMemberRoles",
                columns: new[] { "Group Member Id", "Role Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberRoles_Role Id",
                table: "GroupMemberRoles",
                column: "Role Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupMemberRoles");
        }
    }
}
