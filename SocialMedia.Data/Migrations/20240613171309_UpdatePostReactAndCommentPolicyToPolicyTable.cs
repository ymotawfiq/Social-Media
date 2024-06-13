using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostReactAndCommentPolicyToPolicyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_CommentPolicies_Comment Policy Id",
                table: "Posts");


            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostPolicies_Post Policy Id",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ReactPolicies_React Policy Id",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactPolicies_Policies_Policy Id",
                table: "ReactPolicies");

            migrationBuilder.RenameColumn(
                name: "User Id",
                table: "Posts",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "React Policy Id",
                table: "Posts",
                newName: "ReactPolicyId");

            migrationBuilder.RenameColumn(
                name: "Post Policy Id",
                table: "Posts",
                newName: "PostPolicyId");

            migrationBuilder.RenameColumn(
                name: "Comment Policy Id",
                table: "Posts",
                newName: "CommentPolicyId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_User Id",
                table: "Posts",
                newName: "IX_Posts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_React Policy Id",
                table: "Posts",
                newName: "IX_Posts_ReactPolicyId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_Post Policy Id",
                table: "Posts",
                newName: "IX_Posts_PostPolicyId");


            migrationBuilder.RenameIndex(
                name: "IX_Posts_Comment Policy Id",
                table: "Posts",
                newName: "IX_Posts_CommentPolicyId");


            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");


            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Policies_CommentPolicyId",
                table: "Posts",
                column: "CommentPolicyId",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Policies_PostPolicyId",
                table: "Posts",
                column: "PostPolicyId",
                principalTable: "Policies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Policies_ReactPolicyId",
                table: "Posts",
                column: "ReactPolicyId",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_CommentPolicies_CommentPolicyId1",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Policies_CommentPolicyId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Policies_PostPolicyId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Policies_ReactPolicyId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostPolicies_PostsPolicyId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_ReactPolicies_ReactPolicyId1",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_ReactPolicies_Policies_Policy Id",
                table: "ReactPolicies");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CommentPolicyId1",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_ReactPolicyId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CommentPolicyId1",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ReactPolicyId1",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Posts",
                newName: "User Id");

            migrationBuilder.RenameColumn(
                name: "ReactPolicyId",
                table: "Posts",
                newName: "React Policy Id");

            migrationBuilder.RenameColumn(
                name: "PostPolicyId",
                table: "Posts",
                newName: "Post Policy Id");

            migrationBuilder.RenameColumn(
                name: "CommentPolicyId",
                table: "Posts",
                newName: "Comment Policy Id");

            migrationBuilder.RenameColumn(
                name: "PostsPolicyId",
                table: "Posts",
                newName: "PolicyId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                newName: "IX_Posts_User Id");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_ReactPolicyId",
                table: "Posts",
                newName: "IX_Posts_React Policy Id");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostsPolicyId",
                table: "Posts",
                newName: "IX_Posts_PolicyId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PostPolicyId",
                table: "Posts",
                newName: "IX_Posts_Post Policy Id");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CommentPolicyId",
                table: "Posts",
                newName: "IX_Posts_Comment Policy Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 12, 23, 24, 58, 818, DateTimeKind.Local).AddTicks(2345),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 13, 20, 13, 8, 60, DateTimeKind.Local).AddTicks(9182));

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_CommentPolicies_Comment Policy Id",
                table: "Posts",
                column: "Comment Policy Id",
                principalTable: "CommentPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Policies_PolicyId",
                table: "Posts",
                column: "PolicyId",
                principalTable: "Policies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_PostPolicies_Post Policy Id",
                table: "Posts",
                column: "Post Policy Id",
                principalTable: "PostPolicies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_ReactPolicies_React Policy Id",
                table: "Posts",
                column: "React Policy Id",
                principalTable: "ReactPolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReactPolicies_Policies_Policy Id",
                table: "ReactPolicies",
                column: "Policy Id",
                principalTable: "Policies",
                principalColumn: "Id");
        }
    }
}
