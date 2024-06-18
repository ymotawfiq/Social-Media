using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropSpecialReactTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReacts_SpecialPostReacts_Post React Id",
                table: "PostReacts");

            migrationBuilder.DropTable(
                name: "SpecialCommentReacts");

            migrationBuilder.DropTable(
                name: "SpecialPostReacts");

            migrationBuilder.AlterColumn<string>(
                name: "React Value",
                table: "Reacts",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Reacts_React Value",
                table: "Reacts",
                column: "React Value",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReacts_Reacts_Post React Id",
                table: "PostReacts",
                column: "Post React Id",
                principalTable: "Reacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostReacts_Reacts_Post React Id",
                table: "PostReacts");

            migrationBuilder.DropIndex(
                name: "IX_Reacts_React Value",
                table: "Reacts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 22, 19, 42, 331, DateTimeKind.Local).AddTicks(3750),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 14, 9, 43, 1, 596, DateTimeKind.Local).AddTicks(1542));

            migrationBuilder.AlterColumn<string>(
                name: "React Value",
                table: "Reacts",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "SpecialCommentReacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReactId = table.Column<string>(name: "React Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialCommentReacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialCommentReacts_Reacts_React Id",
                        column: x => x.ReactId,
                        principalTable: "Reacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecialPostReacts",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReactId = table.Column<string>(name: "React Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialPostReacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialPostReacts_Reacts_React Id",
                        column: x => x.ReactId,
                        principalTable: "Reacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SpecialCommentReacts_React Id",
                table: "SpecialCommentReacts",
                column: "React Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SpecialPostReacts_React Id",
                table: "SpecialPostReacts",
                column: "React Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostReacts_SpecialPostReacts_Post React Id",
                table: "PostReacts",
                column: "Post React Id",
                principalTable: "SpecialPostReacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
