using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResumeAI.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCoverLetter1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoverLetters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientFullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RecipientAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyContent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Closing = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignatureName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThirdName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResumeImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverLetters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoverLetterExperiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmploymentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverLetterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverLetterExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverLetterExperiences_CoverLetters_CoverLetterId",
                        column: x => x.CoverLetterId,
                        principalTable: "CoverLetters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverLetterLanguages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProficiencyLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverLetterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverLetterLanguages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverLetterLanguages_CoverLetters_CoverLetterId",
                        column: x => x.CoverLetterId,
                        principalTable: "CoverLetters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoverLetterSkills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SkillDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverLetterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoverLetterSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoverLetterSkills_CoverLetters_CoverLetterId",
                        column: x => x.CoverLetterId,
                        principalTable: "CoverLetters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoverLetterExperiences_CoverLetterId",
                table: "CoverLetterExperiences",
                column: "CoverLetterId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverLetterLanguages_CoverLetterId",
                table: "CoverLetterLanguages",
                column: "CoverLetterId");

            migrationBuilder.CreateIndex(
                name: "IX_CoverLetterSkills_CoverLetterId",
                table: "CoverLetterSkills",
                column: "CoverLetterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoverLetterExperiences");

            migrationBuilder.DropTable(
                name: "CoverLetterLanguages");

            migrationBuilder.DropTable(
                name: "CoverLetterSkills");

            migrationBuilder.DropTable(
                name: "CoverLetters");
        }
    }
}
