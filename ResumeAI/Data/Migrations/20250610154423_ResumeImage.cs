using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class ResumeImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonLasttName",
                table: "AspNetUsers",
                newName: "PersonLastName");

            migrationBuilder.AddColumn<string>(
                name: "ResumeImage",
                table: "Resumes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResumeImage",
                table: "Resumes");

            migrationBuilder.RenameColumn(
                name: "PersonLastName",
                table: "AspNetUsers",
                newName: "PersonLasttName");
        }
    }
}
