using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Interfaces;
using ResumeAI.Models.CreateEmail;

namespace ResumeAI.MyService
{
    public class EmailService : ICreateEmail
    {
        private readonly ApplicationDbContext _context;

        public EmailService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateEmailAsync(CreateEmailDTO createEmailDTO, string userId)
        {
            var createEmail = new CreateEmail
            {
                UserId = userId,
                EmailType = createEmailDTO.EmailType?.Trim(),
                Subject = createEmailDTO.Subject?.Trim(),
                RecipientName = createEmailDTO.RecipientName?.Trim(),
                SenderName = createEmailDTO.SenderName?.Trim(),
                Tone = createEmailDTO.Tone?.Trim(),
                Purpose = createEmailDTO.Purpose?.Trim(),
                AdditionalInfo = createEmailDTO.AdditionalInfo?.Trim()
            };
        }
    }
}
