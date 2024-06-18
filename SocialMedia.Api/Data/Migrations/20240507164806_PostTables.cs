using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class PostTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactPolicies_Policies_Policy Id",
                table: "ReactPolicies");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(name: "Policy Id", type: "nvarchar(450)", nullable: false),
                    ReactPolicyId = table.Column<string>(name: "React Policy Id", type: "nvarchar(450)", nullable: false),
                    CommentPolicyId = table.Column<string>(name: "Comment Policy Id", type: "nvarchar(450)", nullable: false),
                    Postcontent = table.Column<string>(name: "Post content", type: "nvarchar(max)", nullable: false),
                    PostDate = table.Column<DateTime>(name: "Post Date", type: "datetime2", nullable: false),
                    PostUpdateDate = table.Column<DateTime>(name: "Post Update Date", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_CommentPolicies_Comment Policy Id",
                        column: x => x.CommentPolicyId,
                        principalTable: "CommentPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Policies_Policy Id",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Posts_ReactPolicies_React Policy Id",
                        column: x => x.ReactPolicyId,
                        principalTable: "ReactPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostImages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(name: "Post Id", type: "nvarchar(450)", nullable: false),
                    ImageUrl = table.Column<string>(name: "Image Url", type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PostImages_Posts_Post Id",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPosts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(name: "User Id", type: "nvarchar(450)", nullable: false),
                    PostId = table.Column<string>(name: "Post Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPosts_AspNetUsers_User Id",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPosts_Posts_Post Id",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostImages_Post Id",
                table: "PostImages",
                column: "Post Id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Comment Policy Id",
                table: "Posts",
                column: "Comment Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Policy Id",
                table: "Posts",
                column: "Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_React Policy Id",
                table: "Posts",
                column: "React Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_Post Id",
                table: "UserPosts",
                column: "Post Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPosts_User Id",
                table: "UserPosts",
                column: "User Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactPolicies_Policies_Policy Id",
                table: "ReactPolicies",
                column: "Policy Id",
                principalTable: "Policies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReactPolicies_Policies_Policy Id",
                table: "ReactPolicies");

            migrationBuilder.DropTable(
                name: "PostImages");

            migrationBuilder.DropTable(
                name: "UserPosts");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_ReactPolicies_Policies_Policy Id",
                table: "ReactPolicies",
                column: "Policy Id",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
