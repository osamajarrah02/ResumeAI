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
        [Authorize]
        public IActionResult Add()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var portfolioDto = new PortfolioDTO
            {
                UserId = userId,
                Services = new List<ServiceDTO> { new ServiceDTO() },
                Projects = new List<ProjectDTO> { new ProjectDTO() }
            };

            return View(portfolioDto);
        }
        private string ConvertDtoToRawText(PortfolioDTO dto)
        {
            return $@"
            First Name: {dto.FirstName}
            Second Name: {dto.SecondName}
            Third Name: {dto.ThirdName}
            Email: {dto.Email}
            Phone: {dto.PhoneNumber}
            Job Title: {dto.JobTitle}
            Date of Birth: {dto.DateOfBirth}
            Address: {dto.Address}
            Summary: {dto.Summary}

            Services:
            {string.Join("\n", dto.Services.Select(s => $"- {s.ServiceName}: {s.ServiceDescription}"))}

            Projects:
            {string.Join("\n", dto.Projects.Select(p => $"- {p.ProjectName} ({p.StartDate:yyyy-MM-dd} - {p.EndDate:yyyy-MM-dd})\n  Description: {p.ProjectDescription}\n  Link: {p.ProjectLink}\n  Attachments: {p.ProjectAttachments}"))}
            ";
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PortfolioDTO model, string rawPortfolioText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            if (!string.IsNullOrWhiteSpace(rawPortfolioText))
            {
                var portfolioParsed = await _portfolioParser.ParsePortFolioAsync(rawPortfolioText);
                if (portfolioParsed == null)
                {
                    ModelState.AddModelError("", "Failed to parse portfolio with AI.");
                    return View("Add");
                }

                model = new PortfolioDTO
                {
                    FirstName = portfolioParsed.FirstName,
                    SecondName = portfolioParsed.SecondName,
                    ThirdName = portfolioParsed.ThirdName,
                    Email = portfolioParsed.Email,
                    PhoneNumber = portfolioParsed.PhoneNumber,
                    Address = portfolioParsed.Address,
                    Summary = portfolioParsed.Summary,
                    DateOfBirth = portfolioParsed.DateOfBirth,
                    JobTitle = portfolioParsed.JobTitle,
                    Services = portfolioParsed.Services?.Select(s => new ServiceDTO
                    {
                        ServiceName = s.ServiceName,
                        ServiceDescription = s.ServiceDescription
                    }).ToList(),
                    Projects = portfolioParsed.Projects?.Select(p => new ProjectDTO
                    {
                        ProjectName = p.ProjectName,
                        ProjectDescription = p.ProjectDescription,
                        StartDate = p.StartDate,
                        EndDate = p.EndDate,
                        ProjectAttachments = p.ProjectAttachments,
                        ProjectLink = p.ProjectLink
                    }).ToList(),
                    UserId = userId
                };
            }
            await _portfolioService.CreatePortfolioAsync(model, userId);

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
