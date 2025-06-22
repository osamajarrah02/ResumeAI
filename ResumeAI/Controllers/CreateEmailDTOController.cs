using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeAI.DTOs;
using ResumeAI.Interfaces;
using ResumeAI.Models.Email;
using ResumeAI.Models.Person;
using ResumeAI.MyService;

namespace ResumeAI.Controllers
{
    public class CreateEmailDTOController : Controller
    {
        private readonly ICreateEmail _createEmail;
        private readonly UserManager<Person> _userManager;
        private readonly ICreateEmailParser _createEmailParser;

        public CreateEmailDTOController(
            UserManager<Person> userManager,
            ICreateEmail createEmail,
            ICreateEmailParser createEmailParser)
        {
            _userManager = userManager;
            _createEmail = createEmail;
            _createEmailParser = createEmailParser;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User Not Found");

            var createEmailDto = new CreateEmailDTO
            {
                PersonFirstName = user.PersonFirstName,
                PersonLastName = user.PersonLastName
            };

            return View(createEmailDto);
        }

        [Authorize]
        [HttpGet]
        public IActionResult Add()
        {
            return View(new CreateEmailDTO());
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmailDTO model, [FromForm] string rawText)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            CreateEmail createEmail;

            var portfolio = await _createEmailParser.ParseEmailAsync(rawText);

            createEmail = new CreateEmail
            {
                UserId = userId,
                EmailType = model.EmailType,
                Subject = model.Subject,
                RecipientName = model.RecipientName,
                SenderName = model.SenderName,
                Tone = model.Tone,
                Purpose = model.Purpose,
                AdditionalInfo = model.AdditionalInfo
            };

            await _createEmail.SaveGeneratedEmailAsync(userId, createEmail);
            return RedirectToAction("View");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> View()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var model = await _createEmail.GetEmailByUserIdAsync(userId);
            if (model == null)
                return NotFound("Email not found for the given user ID.");

            return View(model);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var model = await _createEmail.GetEmailByUserIdAsync(userId);
            if (model == null)
                return NotFound("Email not found for the given user ID.");

            return View(model);
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateEmailDTO model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var updatedEmail = new CreateEmail
            {
                UserId = userId,
                EmailType = model.EmailType?.Trim(),
                Subject = model.Subject?.Trim(),
                RecipientName = model.RecipientName?.Trim(),
                SenderName = model.SenderName?.Trim(),
                Tone = model.Tone?.Trim(),
                Purpose = model.Purpose?.Trim(),
                AdditionalInfo = model.AdditionalInfo?.Trim()
            };

            var success = await _createEmail.UpdateGeneratedEmailAsync(updatedEmail);
            if (!success)
                return NotFound("No existing email to update.");

            return RedirectToAction("View");
        }
    }
}