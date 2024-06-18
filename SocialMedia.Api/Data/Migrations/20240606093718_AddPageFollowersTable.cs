using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPageFollowersTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageFollowers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PageId = table.Column<string>(name: "Page Id", type: "nvarchar(450)", nullable: false),
                    FollowerId = table.Column<string>(name: "Follower Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageFollowers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageFollowers_AspNetUsers_Follower Id",
                        column: x => x.FollowerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageFollowers_Pages_Page Id",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageFollowers_Follower Id",
                table: "PageFollowers",
                column: "Follower Id");

            migrationBuilder.CreateIndex(
                name: "IX_PageFollowers_Page Id_Follower Id",
                table: "PageFollowers",
                columns: new[] { "Page Id", "Follower Id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageFollowers");
        }
    }
}
