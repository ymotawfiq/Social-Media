using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFriendMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friends");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5312f69a-cc3b-448a-b0de-db27576f2d97");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7932404a-0a05-442b-8d9c-02687251d9f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a3f20d50-137e-4b85-b7e4-86f68ba1a8af");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c170ab54-4fc3-4c38-8dc7-d065528b6113");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c178cfe6-319f-4b13-a31a-1571e052f839");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Friends",
                newName: "User Id");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_UserId",
                table: "Friends",
                newName: "IX_Friends_User Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_User Id",
                table: "Friends",
                column: "User Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Friends_AspNetUsers_User Id",
                table: "Friends");

            migrationBuilder.RenameColumn(
                name: "User Id",
                table: "Friends",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Friends_User Id",
                table: "Friends",
                newName: "IX_Friends_UserId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5312f69a-cc3b-448a-b0de-db27576f2d97", "1", "Admin", "Admin" },
                    { "7932404a-0a05-442b-8d9c-02687251d9f2", "5", "GroupMember", "GroupMember" },
                    { "a3f20d50-137e-4b85-b7e4-86f68ba1a8af", "2", "User", "User" },
                    { "c170ab54-4fc3-4c38-8dc7-d065528b6113", "3", "Owner", "Owner" },
                    { "c178cfe6-319f-4b13-a31a-1571e052f839", "4", "Moderator", "Moderator" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Friends_AspNetUsers_UserId",
                table: "Friends",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
