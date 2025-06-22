using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Resumes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
