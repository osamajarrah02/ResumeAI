using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Interfaces;
using ResumeAI.Models.Email;
using ResumeAI.Models.Portfolio;

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

            _context.CreateEmails.Add(createEmail);
            await _context.SaveChangesAsync();
        }
        public async Task<CreateEmailDTO> GetEmailByUserIdAsync(string userId)
        {
            var model = await _context.CreateEmails
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (model == null) return null;

            return new CreateEmailDTO
            {
                EmailType = model.EmailType,
                Subject = model.Subject,
                RecipientName = model.RecipientName,
                SenderName = model.SenderName,
                Tone = model.Tone,
                Purpose = model.Purpose,
                AdditionalInfo = model.AdditionalInfo,
                UserId = model.UserId,
                User = model.User
            };
        }
    }
}
