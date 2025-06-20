using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeAI.DTOs;
using ResumeAI.DTOs.Details;
using ResumeAI.Interfaces;
using ResumeAI.Models.CoverLetter;
using ResumeAI.Models.CoverLetter.Details;
using ResumeAI.Models.Person;
using ResumeAI.Models.Resume;
using ResumeAI.Models.Resume.Details;
using ResumeAI.MyService;

namespace ResumeAI.Controllers
{
    [Authorize]
    public class CoverLetterDTOController : Controller
    {
        private readonly ICoverLetter _coverLetter;
        private readonly ICoverLetterParser _coverLetterParser;
        
        public CoverLetterDTOController(UserManager<Person> userManager, ICoverLetter coverLetter, ICoverLetterParser coverLetterParser)
        {
            _coverLetter = coverLetter;
            _coverLetterParser = coverLetterParser;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var model = await _coverLetter.GetCoverLetterAsync(userId);
            if (model == null) return NotFound("User not found");

            return View(model);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CoverLetterDTO coverLetterDTO, [FromForm] string rawText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var coverLetter = await _coverLetterParser.ParseCoverLetterAsync(rawText);
            // 🔹 Fallback: manual form-based creation
#pragma warning disable CS8601 // Possible null reference assignment.
            coverLetter = new CoverLetter
            {
                UserId = userId,
                FirstName = coverLetterDTO.FirstName,
                SecondName = coverLetterDTO.SecondName,
                ThirdName = coverLetterDTO.ThirdName,
                Email = coverLetterDTO.Email,
                PhoneNumber = coverLetterDTO.PhoneNumber,
                Address = coverLetterDTO.Address,
                JobTitle = coverLetterDTO.JobTitle,
                Summary = coverLetterDTO.Summary,
                RecipientFullName = coverLetterDTO.RecipientFullName,
                RecipientAddress = coverLetterDTO.RecipientAddress,
                Introduction = coverLetterDTO.Introduction,
                BodyContent = coverLetterDTO.BodyContent,
                Closing = coverLetterDTO.Closing,
                SignatureName = coverLetterDTO.SignatureName,
                CoverLetterExperiences = coverLetterDTO.CoverLetterExperiences?.Select(e => new CoverLetterExperience
                {
                    JobTitle = e.JobTitle?.Trim() ?? string.Empty,
                    CompanyName = e.CompanyName?.Trim() ?? string.Empty,
                    CompanyLocation = e.CompanyLocation?.Trim() ?? string.Empty,
                    StartDate = e.StartDate?.Trim() ?? string.Empty,
                    EndDate = e.EndDate?.Trim() ?? string.Empty,
                    EmploymentType = e.EmploymentType?.Trim() ?? string.Empty,
                    Description = e.Description?.Trim() ?? string.Empty
                }).ToList(),

                CoverLetterSkills = coverLetterDTO.CoverLetterSkills?.Select(s => new CoverLetterSkill
                {
                    SkillName = s.SkillName?.Trim() ?? string.Empty,
                    SkillCategory = s.SkillCategory?.Trim() ?? string.Empty,
                    SkillDescription = s.SkillDescription?.Trim() ?? string.Empty
                }).ToList(),

                CoverLetterLanguages = coverLetterDTO.CoverLetterLanguages?.Select(l => new CoverLetterLanguage
                {
                    LanguageName = l.LanguageName?.Trim() ?? string.Empty,
                    ProficiencyLevel = l.ProficiencyLevel?.Trim() ?? string.Empty
                }).ToList(),
            };
#pragma warning restore CS8601 // Possible null reference assignment.
            await _coverLetter.SaveGeneratedCoverLetterAsync(userId, coverLetter);
            return RedirectToAction("ViewGenerated");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewGenerated()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var model = await _coverLetter.GetCoverLetterModelAsync(userId);
            if (model == null) return NotFound();

            var dto = new CoverLetterDTO
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ThirdName = model.ThirdName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                JobTitle = model.JobTitle,
                Summary = model.Summary,
                RecipientFullName = model.RecipientFullName,
                RecipientAddress = model.RecipientAddress,
                Introduction = model.Introduction,
                BodyContent = model.BodyContent,
                Closing = model.Closing,
                SignatureName = model.SignatureName,

                CoverLetterExperiences = model.CoverLetterExperiences.Select(e => new CoverLetterExperienceDTO
                {
                    JobTitle = e.JobTitle,
                    CompanyName = e.CompanyName,
                    CompanyLocation = e.CompanyLocation,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    EmploymentType = e.EmploymentType,
                    Description = e.Description
                }).ToList(),

                CoverLetterSkills = model.CoverLetterSkills.Select(s => new CoverLetterSkillDTO
                {
                    SkillName = s.SkillName,
                    SkillCategory = s.SkillCategory,
                    SkillDescription = s.SkillDescription
                }).ToList(),

                CoverLetterLanguages = model.CoverLetterLanguages.Select(l => new CoverLetterLanguageDTO
                {
                    LanguageName = l.LanguageName,
                    ProficiencyLevel = l.ProficiencyLevel
                }).ToList(),
            };
            return View(dto);
        }
    }
}
