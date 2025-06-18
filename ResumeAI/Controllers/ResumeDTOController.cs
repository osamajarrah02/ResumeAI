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

        private string ConvertDtoToRawText(ResumeDTO dto)
        {
            return $@"
            First Name: {dto.FirstName}
            Second Name: {dto.SecondName}
            Third Name: {dto.LastName}
            Email: {dto.Email}
            Phone: {dto.PhoneNumber}
            Job Title: {dto.JobTitle}
            Date of Birth: {dto.DateOfBirth}
            Address: {dto.Address}
            Summary: {dto.Summary}

            Education:
            {string.Join("\n", dto.Educations.Select(e => $"{e.Degree} in {e.FieldOfStudy} from {e.InstitutionName} ({e.StartDate} - {e.EndDate}, GPA: {e.GPA}, Description: {e.Description})"))}

            Experience:
            {string.Join("\n", dto.Experiences.Select(e => $"{e.JobTitle} at {e.CompanyName}, {e.CompanyLocation} ({e.StartDate} - {e.EndDate}) - {e.EmploymentType}, Current: {e.IsCurrent}, Description: {e.Description}"))}

            Certificates:
            {string.Join("\n", dto.Certificates.Select(c => $"{c.CourseName} - {c.InstitutionName}, GPA: {c.GPA}, Type: {c.CertificateType}, ({c.StartDate} - {c.EndDate})"))}

            Skills:
            {string.Join(", ", dto.Skills.Select(s => $"{s.SkillName} ({s.SkillCategory}) - {s.SkillDescription}"))}

            Languages:
            {string.Join(", ", dto.Languages.Select(l => $"{l.LanguageName} ({l.ProficiencyLevel}){(l.IsNative ? " [Native]" : "")}"))}

            Links:
            {string.Join(", ", dto.Links.Select(l => $"{l.LinkName}: {l.LinkUrl}"))}
            ";
        }
        [HttpGet]
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ResumeDTO model, string? rawResumeText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            Resume resume;

            // 🔹 If raw resume text is provided, use AI to parse it
            if (!string.IsNullOrWhiteSpace(rawResumeText))
            {
                var parsed = await _resumeParser.ParseResumeAsync(rawResumeText);
                if (parsed == null)
                {
                    ModelState.AddModelError("", "AI failed to parse the resume. Please try again.");
                    return View(model);
                }

                resume = parsed;
                resume.UserId = userId;
                resume.ResumeCreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
                resume.ResumeModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            }
            else
            {
                // 🔹 Fallback: manual form-based creation
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
            }

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
