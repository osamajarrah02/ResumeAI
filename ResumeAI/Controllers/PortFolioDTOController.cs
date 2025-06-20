using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeAI.DTOs;
using ResumeAI.DTOs.Details;
using ResumeAI.Interfaces;
using ResumeAI.Models.Person;
using ResumeAI.Models.Portfolio;

namespace ResumeAI.Controllers
{
    public class PortFolioDTOController : Controller
    {
        private readonly IPortfolioService _portfolioService;
        private readonly UserManager<Person> _userManager;
        private readonly IPortfolioParser _portfolioParser;

        public PortFolioDTOController(IPortfolioService portfolioService, UserManager<Person> userManager, IPortfolioParser portfolioParser)
        {
            _portfolioService = portfolioService;
            _userManager = userManager;
            _portfolioParser = portfolioParser;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User not found.");

            var portfolioDto = new PortfolioDTO
            {
                PersonFirstName = user.PersonFirstName,
                PersonLastName = user.PersonLastName,
                UserId = user.Id
            };

            return View(portfolioDto);
        }

        [HttpGet]
        public IActionResult Add()
        {
            var portfolioDto = new PortfolioDTO
            {
                Services = new List<ServiceDTO> { new ServiceDTO() },
                Projects = new List<ProjectDTO> { new ProjectDTO() }
            };
            return View(portfolioDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PortfolioDTO model, [FromForm] string rawText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            byte[]? imageBytes = null;

            if (model.PortFolioImage != null && model.PortFolioImage.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await model.PortFolioImage.CopyToAsync(ms);
                    imageBytes = ms.ToArray();
                }
            }

            var portfolio = new Portfolio
            {
                UserId = userId,
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ThirdName = model.ThirdName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                JobTitle = model.JobTitle,
                DateOfBirth = model.DateOfBirth,
                Summary = model.Summary,
                PersonalImage = imageBytes, // ✅ Save image
                Services = model.Services?.Select(e =>
                {
                    byte[]? imageData = null;
                    if (e.ServiceImageFile != null && e.ServiceImageFile.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            e.ServiceImageFile.CopyTo(ms);
                            imageData = ms.ToArray();
                        }
                    }

                    return new ResumeAI.Models.Portfolio.Service
                    {
                        ServiceName = e.ServiceName,
                        ServiceDescription = e.ServiceDescription,
                        ServiceImage = imageData
                    };
                }).ToList(),
                Projects = model.Projects?.Select(c => new Project
                {
                    ProjectName = c.ProjectName,
                    ProjectDescription = c.ProjectDescription,
                    ProjectAttachments = c.ProjectAttachments,
                    ProjectLink = c.ProjectLink,
                }).ToList(),
                CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd"),
                ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd")
            };

            await _portfolioService.SaveGeneratedPortFolioAsync(userId, portfolio);
            return RedirectToAction("View");
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> View()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var model = await _portfolioService.GetPortfolioByUserIdAsync(userId);
            if (model == null) return NotFound("Portfolio not found for the given user ID.");

            var dto = new PortfolioDTO
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ThirdName = model.ThirdName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Summary = model.Summary,
                DateOfBirth = model.DateOfBirth,
                JobTitle = model.JobTitle,
                UserId = model.UserId,
                ImageBase64 = model.PersonalImage != null
                ? $"data:image/png;base64,{Convert.ToBase64String(model.PersonalImage)}"
                : null,
                Services = model.Services?.Select(s => new ServiceDTO
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    ServiceDescription = s.ServiceDescription,
                    ServiceImage = s.ServiceImage
                }).ToList(),

                Projects = model.Projects?.Select(p => new ProjectDTO
                {
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProjectAttachments = p.ProjectAttachments,
                    ProjectLink = p.ProjectLink
                }).ToList()
            };
            return View(dto);
        }

    }
}
