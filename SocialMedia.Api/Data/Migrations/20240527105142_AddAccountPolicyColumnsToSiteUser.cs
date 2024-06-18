using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountPolicyColumnsToSiteUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountPolicies_User Account Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_FriendListPolicies_AspNetUsers_User Id",
                table: "FriendListPolicies");

            migrationBuilder.DropIndex(
                name: "IX_FriendListPolicies_Policy Id",
                table: "FriendListPolicies");

            migrationBuilder.DropIndex(
                name: "IX_FriendListPolicies_User Id",
                table: "FriendListPolicies");

            migrationBuilder.DropColumn(
                name: "User Id",
                table: "FriendListPolicies");

            migrationBuilder.RenameColumn(
                name: "User Account Policy Id",
                table: "AspNetUsers",
                newName: "Account Policy Id");

            migrationBuilder.RenameColumn(
                name: "Is Friend List Private",
                table: "AspNetUsers",
                newName: "IsFriendListPrivate");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_User Account Policy Id",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_Account Policy Id");

            migrationBuilder.AlterColumn<string>(
                name: "Account Policy Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<bool>(
                name: "IsFriendListPrivate",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Comment Policy Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Friend list Policy Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "React Policy Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendListPolicies_Policy Id",
                table: "FriendListPolicies",
                column: "Policy Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Comment Policy Id",
                table: "AspNetUsers",
                column: "Comment Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Friend list Policy Id",
                table: "AspNetUsers",
                column: "Friend list Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_React Policy Id",
                table: "AspNetUsers",
                column: "React Policy Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountPolicies_Account Policy Id",
                table: "AspNetUsers",
                column: "Account Policy Id",
                principalTable: "AccountPolicies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_CommentPolicies_Comment Policy Id",
                table: "AspNetUsers",
                column: "Comment Policy Id",
                principalTable: "CommentPolicies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_FriendListPolicies_Friend list Policy Id",
                table: "AspNetUsers",
                column: "Friend list Policy Id",
                principalTable: "FriendListPolicies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ReactPolicies_React Policy Id",
                table: "AspNetUsers",
                column: "React Policy Id",
                principalTable: "ReactPolicies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountPolicies_Account Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_CommentPolicies_Comment Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_FriendListPolicies_Friend list Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ReactPolicies_React Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_FriendListPolicies_Policy Id",
                table: "FriendListPolicies");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Comment Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Friend list Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_React Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Comment Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Friend list Policy Id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "React Policy Id",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "IsFriendListPrivate",
                table: "AspNetUsers",
                newName: "Is Friend List Private");

            migrationBuilder.RenameColumn(
                name: "Account Policy Id",
                table: "AspNetUsers",
                newName: "User Account Policy Id");

            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_Account Policy Id",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_User Account Policy Id");

            migrationBuilder.AddColumn<string>(
                name: "User Id",
                table: "FriendListPolicies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "Is Friend List Private",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "User Account Policy Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendListPolicies_Policy Id",
                table: "FriendListPolicies",
                column: "Policy Id");

            migrationBuilder.CreateIndex(
                name: "IX_FriendListPolicies_User Id",
                table: "FriendListPolicies",
                column: "User Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountPolicies_User Account Policy Id",
                table: "AspNetUsers",
                column: "User Account Policy Id",
                principalTable: "AccountPolicies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FriendListPolicies_AspNetUsers_User Id",
                table: "FriendListPolicies",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
