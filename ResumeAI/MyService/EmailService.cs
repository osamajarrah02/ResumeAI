using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Interfaces;
using ResumeAI.Models.Email;

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
        public async Task SaveGeneratedEmailAsync(string userId, CreateEmail createEmail)
        {
            EnsureRequiredFieldsAreTrimmed(createEmail);

            var existing = await _context.CreateEmails
                .FirstOrDefaultAsync(e => e.UserId == userId);

            if (existing != null)
            {
                existing.EmailType = createEmail.EmailType;
                existing.Subject = createEmail.Subject;
                existing.RecipientName = createEmail.RecipientName;
                existing.SenderName = createEmail.SenderName;
                existing.Tone = createEmail.Tone;
                existing.Purpose = createEmail.Purpose;
                existing.AdditionalInfo = createEmail.AdditionalInfo;
            }
            else
            {
                _context.CreateEmails.Add(createEmail);
            }

            await _context.SaveChangesAsync();
        }
        public Task UpdateEmailAsync(CreateEmailDTO createEmailDTO)
        {
            throw new NotImplementedException();
        }
        private void EnsureRequiredFieldsAreTrimmed(CreateEmail email)
        {
            email.EmailType = email.EmailType?.Trim();
            email.Subject = email.Subject?.Trim();
            email.RecipientName = email.RecipientName?.Trim();
            email.SenderName = email.SenderName?.Trim();
            email.Tone = email.Tone?.Trim();
            email.Purpose = email.Purpose?.Trim();
            email.AdditionalInfo = email.AdditionalInfo?.Trim();
        }
    }
}
