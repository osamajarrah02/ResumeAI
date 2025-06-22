using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeAI.DTOs;
using ResumeAI.Models;
using ResumeAI.Models.Person;
using ResumeAI.Data;

namespace ResumeAI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Person> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<Person> userManager, ApplicationDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Creator()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            var dto = new ProfileDTO
            {
                FullName = $"{user.PersonFirstName} {user.PersonLastName}",
                Email = user.Email,
                ImageBase64 = user.ProfileImage != null
                    ? $"data:image/png;base64,{Convert.ToBase64String(user.ProfileImage)}"
                    : null,
                ResumeCount = await _context.Resumes.CountAsync(r => r.UserId == userId && !r.IsDeleted),
                PortfolioCount = await _context.Portfolios.CountAsync(p => p.UserId == userId && !p.IsDeleted),
                CoverLetterCount = await _context.CoverLetters.CountAsync(c => c.UserId == userId),
                EmailCount = await _context.CreateEmails.CountAsync(e => e.UserId == userId)
            };

            return View(dto);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadProfileImage(IFormFile profileImage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || profileImage == null || profileImage.Length == 0)
                return RedirectToAction("Profile");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            using (var memoryStream = new MemoryStream())
            {
                await profileImage.CopyToAsync(memoryStream);
                user.ProfileImage = memoryStream.ToArray();
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Message"] = "Profile image updated successfully.";
            }
            else
            {
                TempData["Message"] = "Error updating profile image.";
            }

            return RedirectToAction("Profile");
        }
        [Authorize]
        public async Task<IActionResult> Document()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) return RedirectToAction("Login", "Account");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            var resumes = await _context.Resumes
                .Where(r => r.UserId == userId && !r.IsDeleted)
                .ToListAsync();

            var portfolios = await _context.Portfolios
                .Where(p => p.UserId == userId && !p.IsDeleted)
                .ToListAsync();

            var coverLetters = await _context.CoverLetters
                .Where(c => c.UserId == userId)
                .ToListAsync();

            var emails = await _context.CreateEmails
                .Where(e => e.UserId == userId)
                .ToListAsync();

            var model = new DocumentDTO
            {
                FullName = $"{user.PersonFirstName} {user.PersonLastName}",

                Resumes = resumes.Select(r => new ResumeDTO
                {
                    PersonFirstName = user.PersonFirstName,
                    PersonLastName = user.PersonLastName,
                    FirstName = r.FirstName,
                    SecondName = r.SecondName,
                    LastName = r.ThirdName,
                    Title = r.JobTitle,
                    Email = r.Email,
                    PhoneNumber = r.PhoneNumber,
                    Summary = r.Summary,
                    Address = r.Address,
                    DateOfBirth = r.DateOfBirth,
                    JobTitle = r.JobTitle,
                    Id = r.Id.ToString(),
                    ResumeCreatedDate = string.IsNullOrEmpty(r.ResumeCreatedDate) ? null : DateTime.Parse(r.ResumeCreatedDate),
                    ResumeModifiedDate = string.IsNullOrEmpty(r.ResumeModifiedDate) ? null : DateTime.Parse(r.ResumeModifiedDate)
                }).ToList(),

                Portfolios = portfolios.Select(p =>
                {
                    DateTime? createdDate = DateTime.TryParse(p.CreatedDate, out var cd) ? cd : (DateTime?)null;
                    DateTime? modifiedDate = DateTime.TryParse(p.ModifiedDate, out var md) ? md : (DateTime?)null;

                    return new PortfolioDTO
                    {
                        PersonFirstName = user.PersonFirstName,
                        PersonLastName = user.PersonLastName,
                        FirstName = p.FirstName,
                        SecondName = p.SecondName,
                        ThirdName = p.ThirdName,
                        Email = p.Email,
                        PhoneNumber = p.PhoneNumber,
                        Summary = p.Summary,
                        Address = p.Address,
                        DateOfBirth = p.DateOfBirth,
                        JobTitle = p.JobTitle,
                        Id = p.Id.ToString(),
                        UserId = p.UserId,
                        PortFolioCreatedDate = createdDate,
                        PortFolioModifiedDate = modifiedDate,
                        PersonalImage = p.PersonalImage
                    };
                }).ToList(),

                CoverLetters = coverLetters.Select(c => new CoverLetterDTO
                {
                    PersonFirstName = user.PersonFirstName,
                    PersonLastName = user.PersonLastName,
                    FirstName = c.FirstName,
                    SecondName = c.SecondName,
                    ThirdName = c.ThirdName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Summary = c.Summary,
                    Address = c.Address,
                    JobTitle = c.JobTitle,
                    RecipientFullName = c.RecipientFullName,
                    RecipientAddress = c.RecipientAddress,
                    Date = c.Date,
                    Introduction = c.Introduction,
                    BodyContent = c.BodyContent,
                    Closing = c.Closing,
                    SignatureName = c.SignatureName
                }).ToList(),

                Emails = emails.Select(e => new CreateEmailDTO
                {
                    PersonFirstName = user.PersonFirstName,
                    PersonLastName = user.PersonLastName,
                    EmailType = e.EmailType,
                    Subject = e.Subject,
                    RecipientName = e.RecipientName,
                    SenderName = e.SenderName,
                    Tone = e.Tone,
                    Purpose = e.Purpose,
                    AdditionalInfo = e.AdditionalInfo,
                    UserId = e.UserId
                }).ToList()
            };

            return View(model);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}