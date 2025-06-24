using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Models;
using ResumeAI.Models.Person;

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
        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        [Authorize]
        public async Task<IActionResult> Setting()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return RedirectToAction("Login", "Account");

            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return NotFound();

            var dto = new SettingDTO
            {
                FirstName = user.PersonFirstName,
                LastName = user.PersonLastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View(dto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setting(SettingDTO model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound();

            _logger.LogInformation("User {UserId} updated their profile.", user.Id);

            user.PersonFirstName = model.FirstName;
            user.PersonLastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

                return View(model);
            }
            if (!string.IsNullOrEmpty(model.NewPassword) && !string.IsNullOrEmpty(model.ConfirmPassword))
            {
                if (model.NewPassword == model.ConfirmPassword)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResult = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);
                    if (!passwordResult.Succeeded)
                    {
                        foreach (var error in passwordResult.Errors)
                            ModelState.AddModelError(string.Empty, error.Description);

                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("ConfirmPassword", "Passwords do not match.");
                    return View(model);
                }
            }
            await _context.SaveChangesAsync();

            TempData["Message"] = "Profile updated successfully.";
            return RedirectToAction("Setting");
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
                    : null
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
            TempData["Message"] = result.Succeeded
                ? "Profile image updated successfully."
                : "Error updating profile image.";

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