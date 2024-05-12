using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPostViewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostViews",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(name: "Post Id", type: "nvarchar(450)", nullable: false),
                    Postviewnumber = table.Column<int>(name: "Post view number", type: "int", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostViews_AspNetUsers_User Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostViews_Posts_Post Id",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_Post Id",
                table: "PostViews",
                column: "Post Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostViews_User Id",
                table: "PostViews",
                column: "User Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostViews");
        }
    }
}
