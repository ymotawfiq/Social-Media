using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePolicyColumnToPostPolicyInPostsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Policies_Policy Id",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Policy Id",
                table: "Posts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 12, 23, 4, 58, 175, DateTimeKind.Local).AddTicks(3105),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 11, 20, 12, 47, 402, DateTimeKind.Local).AddTicks(8824));

            migrationBuilder.AlterColumn<string>(
                name: "PolicyId",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "Post Policy Id",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Post Policy Id",
                table: "Posts",
                column: "Post Policy Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Policies_PolicyId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_PostPolicies_Post Policy Id",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_Post Policy Id",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Post Policy Id",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PolicyId",
                table: "Posts",
                newName: "Policy Id");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_PolicyId",
                table: "Posts",
                newName: "IX_Posts_Policy Id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 11, 20, 12, 47, 402, DateTimeKind.Local).AddTicks(8824),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 12, 23, 4, 58, 175, DateTimeKind.Local).AddTicks(3105));

            migrationBuilder.AlterColumn<string>(
                name: "Policy Id",
                table: "Posts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_User Id",
                table: "Posts",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Policies_Policy Id",
                table: "Posts",
                column: "Policy Id",
                principalTable: "Policies",
                principalColumn: "Id");
        }
    }
}
