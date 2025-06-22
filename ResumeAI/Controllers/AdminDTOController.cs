using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Models.Person;

namespace ResumeAI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDTOController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;

        public AdminDTOController(ApplicationDbContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Dashboard()
        {
            var users = await _userManager.Users.OfType<User>().ToListAsync();
            var resumes = await _context.Resumes.ToListAsync();
            var portfolios = await _context.Portfolios.ToListAsync();
            var coverLetters = await _context.CoverLetters.ToListAsync();
            var emails = await _context.CreateEmails.ToListAsync();

            var model = new AdminDashboardDTO
            {
                Users = users,
                ResumesCount = resumes.Count,
                PortfoliosCount = portfolios.Count,
                CoverLettersCount = coverLetters.Count,
                EmailsCount = emails.Count
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> BanUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction("Dashboard");
        }
    }
}
