using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSavedPostsAndUserSavedPostsFldersTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserSavedPostsFolders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    FolderName = table.Column<string>(name: "Folder Name", type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSavedPostsFolders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSavedPostsFolders_AspNetUsers_User Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedPosts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(name: "Post Id", type: "nvarchar(450)", nullable: false),
                    FolderId = table.Column<string>(name: "Folder Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SavedPosts_AspNetUsers_User Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavedPosts_Posts_Post Id",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SavedPosts_UserSavedPostsFolders_Folder Id",
                        column: x => x.FolderId,
                        principalTable: "UserSavedPostsFolders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_Folder Id",
                table: "SavedPosts",
                column: "Folder Id");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_Post Id",
                table: "SavedPosts",
                column: "Post Id");

            migrationBuilder.CreateIndex(
                name: "IX_SavedPosts_User Id",
                table: "SavedPosts",
                column: "User Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedPostsFolders_User Id",
                table: "UserSavedPostsFolders",
                column: "User Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SavedPosts");

            migrationBuilder.DropTable(
                name: "UserSavedPostsFolders");
        }
    }
}
