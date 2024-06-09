using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGroupMemberRolesTable1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_Member Id",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_GroupRoles_Role Id",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_Member Id",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_Role Id_Member Id_Group Id",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "Role Id",
                table: "GroupMembers");

            migrationBuilder.RenameColumn(
                name: "Member Id",
                table: "GroupMembers",
                newName: "User Id");

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
                name: "IX_GroupMembers_User Id_Group Id",
                table: "GroupMembers",
                columns: new[] { "User Id", "Group Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberRoles_Group Member Id",
                table: "GroupMemberRoles",
                column: "Group Member Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMemberRoles_Role Id_Group Member Id",
                table: "GroupMemberRoles",
                columns: new[] { "Role Id", "Group Member Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_User Id",
                table: "GroupMembers",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_AspNetUsers_User Id",
                table: "GroupMembers");

            migrationBuilder.DropTable(
                name: "GroupMemberRoles");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_User Id_Group Id",
                table: "GroupMembers");

            migrationBuilder.RenameColumn(
                name: "User Id",
                table: "GroupMembers",
                newName: "Member Id");

            migrationBuilder.AddColumn<string>(
                name: "Role Id",
                table: "GroupMembers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_Member Id",
                table: "GroupMembers",
                column: "Member Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_Role Id_Member Id_Group Id",
                table: "GroupMembers",
                columns: new[] { "Role Id", "Member Id", "Group Id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_AspNetUsers_Member Id",
                table: "GroupMembers",
                column: "Member Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_GroupRoles_Role Id",
                table: "GroupMembers",
                column: "Role Id",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
