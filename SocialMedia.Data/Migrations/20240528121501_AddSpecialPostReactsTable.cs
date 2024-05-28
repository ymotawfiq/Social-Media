using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSpecialPostReactsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddPrimaryKey(
            name: "PK_Reacts",
            table: "Reacts",
            column: "Id");

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
                name: "IX_SpecialPostReacts_React Id",
                table: "SpecialPostReacts",
                column: "React Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialPostReacts");


        }
    }
}
