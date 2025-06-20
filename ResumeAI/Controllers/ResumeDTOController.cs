using System.Security.Claims;
using HTU_FinalProject.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResumeAI.DTOs;
using ResumeAI.DTOs.Details;
using ResumeAI.Interfaces;
using ResumeAI.Models.Resume;
using ResumeAI.Models.Resume.Details;
using ResumeAI.MyService;

namespace ResumeAI.Controllers
{
    public class ResumeDTOController : Controller
    {
        private readonly IResume _resumeService;
        private readonly IResumeParser _resumeParser;

        public ResumeDTOController(IResume resumeService, IResumeParser resumeParser)
        {
            _resumeService = resumeService;
            _resumeParser = resumeParser;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var model = await _resumeService.GetResumeAsync(userId);
            if (model == null) return NotFound("User not found");

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ResumeDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            if (model.ImageFile == null || model.ImageFile.Length == 0)
            {
                ModelState.AddModelError("ImageFile", "Please upload a valid image file.");
                var refreshedModel = await _resumeService.GetResumeAsync(userId);
                return View(refreshedModel);
            }

            var success = await _resumeService.UpdateProfileImageAsync(userId, model.ImageFile);
            if (!success) return NotFound("User not found");

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ResumeDTO model, [FromForm] string rawText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var resume = await _resumeParser.ParseResumeAsync(rawText);
                // 🔹 Fallback: manual form-based creation
#pragma warning disable CS8601 // Possible null reference assignment.
                resume = new Resume
                {
                    UserId = userId,
                    FirstName = model.FirstName,
                    SecondName = model.SecondName,
                    ThirdName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    JobTitle = model.JobTitle,
                    DateOfBirth = model.DateOfBirth,
                    Summary = model.Summary,
                    Educations = model.Educations?.Select(e => new Education
                    {
                        InstitutionName = e.InstitutionName,
                        Degree = e.Degree,
                        FieldOfStudy = e.FieldOfStudy,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        GPA = e.GPA,
                        Description = e.Description,
                        IsCompleted = e.IsCompleted
                    }).ToList(),
                    Certificates = model.Certificates?.Select(c => new Certificate
                    {
                        CourseName = c.CourseName,
                        InstitutionName = c.InstitutionName,
                        GPA = c.GPA,
                        CertificateType = c.CertificateType,
                        StartDate = c.StartDate,
                        EndDate = c.EndDate
                    }).ToList(),
                    Experiences = model.Experiences?.Select(x => new Experience
                    {
                        JobTitle = x.JobTitle,
                        CompanyName = x.CompanyName,
                        CompanyLocation = x.CompanyLocation,
                        StartDate = x.StartDate,
                        EndDate = x.EndDate,
                        EmploymentType = x.EmploymentType,
                        Description = x.Description,
                        IsCurrent = x.IsCurrent
                    }).ToList(),
                    Skills = model.Skills?.Select(s => new Skill
                    {
                        SkillName = s.SkillName,
                        SkillCategory = s.SkillCategory,
                        SkillDescription = s.SkillDescription
                    }).ToList(),
                    Languages = model.Languages?.Select(l => new Language
                    {
                        LanguageName = l.LanguageName,
                        ProficiencyLevel = l.ProficiencyLevel,
                        IsNative = l.IsNative
                    }).ToList(),
                    Links = model.Links?.Select(l => new Link
                    {
                        LinkName = l.LinkName,
                        LinkUrl = l.LinkUrl
                    }).ToList(),
                    ResumeCreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                    ResumeModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
                };
#pragma warning restore CS8601 // Possible null reference assignment.

            await _resumeService.SaveGeneratedResumeAsync(userId, resume);
            return RedirectToAction("ViewGenerated");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ViewGenerated()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var model = await _resumeService.GetResumeModelAsync(userId);
            if (model == null) return NotFound();

            var dto = new ResumeDTO
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                LastName = model.ThirdName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                JobTitle = model.JobTitle,
                DateOfBirth = model.DateOfBirth,
                Summary = model.Summary,

                Educations = model.Educations.Select(e => new EducationDTO
                {
                    InstitutionName = e.InstitutionName,
                    Degree = e.Degree,
                    FieldOfStudy = e.FieldOfStudy,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    GPA = e.GPA,
                    Description = e.Description,
                    IsCompleted = e.IsCompleted
                }).ToList(),

                Experiences = model.Experiences.Select(e => new ExperienceDTO
                {
                    JobTitle = e.JobTitle,
                    CompanyName = e.CompanyName,
                    CompanyLocation = e.CompanyLocation,
                    StartDate = e.StartDate,
                    EndDate = e.EndDate,
                    IsCurrent = e.IsCurrent,
                    EmploymentType = e.EmploymentType,
                    Description = e.Description
                }).ToList(),

                Certificates = model.Certificates.Select(c => new CertificateDTO
                {
                    CourseName = c.CourseName,
                    InstitutionName = c.InstitutionName,
                    GPA = c.GPA,
                    CertificateType = c.CertificateType,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate
                }).ToList(),

                Languages = model.Languages.Select(l => new LanguageDTO
                {
                    LanguageName = l.LanguageName,
                    ProficiencyLevel = l.ProficiencyLevel,
                    IsNative = l.IsNative
                }).ToList(),

                Skills = model.Skills.Select(s => new SkillDTO
                {
                    SkillName = s.SkillName,
                    SkillCategory = s.SkillCategory,
                    SkillDescription = s.SkillDescription
                }).ToList(),

                Links = model.Links.Select(l => new LinkDTO
                {
                    LinkName = l.LinkName,
                    LinkUrl = l.LinkUrl
                }).ToList()
            };
            return View(dto);
        }
    }
}
