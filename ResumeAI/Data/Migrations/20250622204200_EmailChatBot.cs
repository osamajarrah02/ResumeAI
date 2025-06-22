using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class EmailChatBot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GeneratedEmailContent",
                table: "CreateEmails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GeneratedEmailContent",
                table: "CreateEmails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
