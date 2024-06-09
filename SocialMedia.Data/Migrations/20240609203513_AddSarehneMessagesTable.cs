using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SocialMedia.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddSarehneMessagesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SarehneMessages",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(name: "Receiver Id", type: "nvarchar(450)", nullable: false),
                    SenderName = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "Anonymous"),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SarehneMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SarehneMessages_AspNetUsers_Receiver Id",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SarehneMessages_Receiver Id",
                table: "SarehneMessages",
                column: "Receiver Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SarehneMessages");
        }
    }
}
