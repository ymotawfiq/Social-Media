using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class DropSarehneMessagePolicyTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SarehneMessages_SarehneMessagePolicies_Message Policy Id",
                table: "SarehneMessages");

            migrationBuilder.DropTable(
                name: "SarehneMessagePolicies");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 22, 2, 45, 756, DateTimeKind.Local).AddTicks(8855),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 13, 21, 46, 9, 97, DateTimeKind.Local).AddTicks(6840));

            migrationBuilder.AddForeignKey(
                name: "FK_SarehneMessages_Policies_Message Policy Id",
                table: "SarehneMessages",
                column: "Message Policy Id",
                principalTable: "Policies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SarehneMessages_Policies_Message Policy Id",
                table: "SarehneMessages");

            migrationBuilder.AlterColumn<DateTime>(
                name: "SentAt",
                table: "SarehneMessages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2024, 6, 13, 21, 46, 9, 97, DateTimeKind.Local).AddTicks(6840),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2024, 6, 13, 22, 2, 45, 756, DateTimeKind.Local).AddTicks(8855));

            migrationBuilder.CreateTable(
                name: "SarehneMessagePolicies",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PolicyId = table.Column<string>(name: "Policy Id", type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SarehneMessagePolicies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SarehneMessagePolicies_Policies_Policy Id",
                        column: x => x.PolicyId,
                        principalTable: "Policies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SarehneMessagePolicies_Policy Id",
                table: "SarehneMessagePolicies",
                column: "Policy Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SarehneMessages_SarehneMessagePolicies_Message Policy Id",
                table: "SarehneMessages",
                column: "Message Policy Id",
                principalTable: "SarehneMessagePolicies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
