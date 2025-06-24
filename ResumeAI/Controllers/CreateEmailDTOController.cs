using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ResumeAI.DTOs;
using ResumeAI.Interfaces;
using ResumeAI.Models.Person;

namespace ResumeAI.Controllers
{
    [Authorize]
    public class CreateEmailDTOController : Controller
    {
        private readonly ICreateEmail _createEmail;
        private readonly UserManager<Person> _userManager;
        private readonly ICreateEmailParser _createEmailParser;

        public CreateEmailDTOController(
            UserManager<Person> userManager,
            ICreateEmailParser createEmailParser,
            ICreateEmail createEmail)
        {
            _userManager = userManager;
            _createEmailParser = createEmailParser;
            _createEmail = createEmail;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return NotFound("User Not Found");

            var model = new CreateEmailDTO
            {
                PersonFirstName = user.PersonFirstName,
                PersonLastName = user.PersonLastName
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult Chat()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Chat([FromForm] string userMessage)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (string.IsNullOrWhiteSpace(userMessage))
                return Json(new { reply = "Please enter a message." });

            try
            {
                var aiResponse = await _createEmailParser.ParseEmailAsync(userMessage);

                if (string.IsNullOrWhiteSpace(aiResponse))
                {
                    return Json(new { reply = "Sorry, I didn't understand that." });
                }

                return Json(new { reply = aiResponse.Trim() });
            }
            catch
            {
                return Json(new { reply = "⚠️ Error contacting server. Please try again." });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitEmailDetails([FromBody] CreateEmailDTO emailDetails)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (emailDetails == null)
                return BadRequest(new { success = false, message = "Email details are required." });

            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Invalid email details." });

            await _createEmail.CreateEmailAsync(emailDetails, userId);

            return Json(new { success = true, message = "Email saved successfully." });
        }
    }
}