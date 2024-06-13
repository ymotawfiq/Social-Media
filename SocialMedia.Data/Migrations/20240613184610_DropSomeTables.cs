using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropSomeTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropTable(
                name: "AccountPolicies");

            migrationBuilder.DropTable(
                name: "CommentPolicies");

            migrationBuilder.DropTable(
                name: "FriendListPolicies");

            migrationBuilder.DropTable(
                name: "PostPolicies");

            migrationBuilder.DropTable(
                name: "ReactPolicies");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
