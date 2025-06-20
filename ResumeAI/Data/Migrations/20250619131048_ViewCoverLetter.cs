using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ViewCoverLetter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CoverLetters",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CoverLetters_UserId",
                table: "CoverLetters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoverLetters_AspNetUsers_UserId",
                table: "CoverLetters",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoverLetters_AspNetUsers_UserId",
                table: "CoverLetters");

            migrationBuilder.DropIndex(
                name: "IX_CoverLetters_UserId",
                table: "CoverLetters");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CoverLetters");
        }
    }
}
