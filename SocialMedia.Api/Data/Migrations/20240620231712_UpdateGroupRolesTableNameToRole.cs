using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGroupRolesTableNameToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberRoles_GroupRoles_Role Id",
                table: "GroupMemberRoles");

            migrationBuilder.DropTable(
                name: "GroupChatMembers");

            migrationBuilder.DropTable(
                name: "GroupRoles");

            migrationBuilder.DropTable(
                name: "GroupChats");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Role_RoleName",
                table: "Role",
                column: "RoleName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberRoles_Role_Role Id",
                table: "GroupMemberRoles",
                column: "Role Id",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMemberRoles_Role_Role Id",
                table: "GroupMemberRoles");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.CreateTable(
                name: "GroupChats",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedByUserId = table.Column<string>(name: "Created By User Id", type: "nvarchar(450)", nullable: false),
                    GroupName = table.Column<string>(name: "Group Name", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChats_AspNetUsers_Created By User Id",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupChatMembers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    GroupChatId = table.Column<string>(name: "Group Chat Id", type: "nvarchar(450)", nullable: false),
                    MemberId = table.Column<string>(name: "Member Id", type: "nvarchar(450)", nullable: false),
                    IsJoinRequestAccepted = table.Column<bool>(name: "Is Join Request Accepted", type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupChatMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GroupChatMembers_AspNetUsers_Member Id",
                        column: x => x.MemberId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupChatMembers_GroupChats_Group Chat Id",
                        column: x => x.GroupChatId,
                        principalTable: "GroupChats",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMembers_Group Chat Id_Member Id",
                table: "GroupChatMembers",
                columns: new[] { "Group Chat Id", "Member Id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupChatMembers_Member Id",
                table: "GroupChatMembers",
                column: "Member Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupChats_Created By User Id",
                table: "GroupChats",
                column: "Created By User Id");

            migrationBuilder.CreateIndex(
                name: "IX_GroupRoles_RoleName",
                table: "GroupRoles",
                column: "RoleName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMemberRoles_GroupRoles_Role Id",
                table: "GroupMemberRoles",
                column: "Role Id",
                principalTable: "GroupRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
