using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class EditEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CreateEmails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CreateEmails_UserId",
                table: "CreateEmails",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CreateEmails_AspNetUsers_UserId",
                table: "CreateEmails",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CreateEmails_AspNetUsers_UserId",
                table: "CreateEmails");

            migrationBuilder.DropIndex(
                name: "IX_CreateEmails_UserId",
                table: "CreateEmails");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CreateEmails");
        }
    }
}
