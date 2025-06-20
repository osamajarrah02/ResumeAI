using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Interfaces;
using ResumeAI.Models.CoverLetter;
using ResumeAI.Models.Person;
using ResumeAI.Models.Resume;

namespace ResumeAI.MyService
{
    public class CoverLetterService : ICoverLetter
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Person> _userManager;

        public CoverLetterService(ApplicationDbContext context, UserManager<Person> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<CoverLetterDTO?> GetCoverLetterAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return null;

            return new CoverLetterDTO
            {
                PersonFirstName = user.PersonFirstName,
                PersonLastName = user.PersonLastName,
            };
        }

        public async Task<CoverLetter?> GetCoverLetterModelAsync(string userId)
        {
            return await _context.CoverLetters
            .Include(r => r.CoverLetterExperiences)
            .Include(r => r.CoverLetterSkills)
            .Include(r => r.CoverLetterLanguages)
            .FirstOrDefaultAsync(r => r.UserId == userId);
        }
        public async Task SaveGeneratedCoverLetterAsync(string userId, CoverLetter coverLetter)
        {
            EnsureRequiredFieldsAreFilled(coverLetter);
            var existing = await _context.CoverLetters
            .Include(r => r.CoverLetterExperiences)
            .Include(r => r.CoverLetterSkills)
            .Include(r => r.CoverLetterLanguages)
            .FirstOrDefaultAsync(r => r.UserId == userId);

            if (existing != null)
            {
                existing.FirstName = coverLetter.FirstName;
                existing.SecondName = coverLetter.SecondName;
                existing.ThirdName = coverLetter.ThirdName;
                existing.Email = coverLetter.Email;
                existing.PhoneNumber = coverLetter.PhoneNumber;
                existing.Address = coverLetter.Address;
                existing.JobTitle = coverLetter.JobTitle;
                existing.DateOfBirth = coverLetter.DateOfBirth;
                existing.Summary = coverLetter.Summary;
                existing.RecipientFullName = coverLetter.RecipientFullName;
                existing.RecipientAddress = coverLetter.RecipientAddress;
                existing.Introduction = coverLetter.Introduction;
                existing.BodyContent = coverLetter.BodyContent;
                existing.Closing = coverLetter.Closing;
                existing.SignatureName = coverLetter.SignatureName;

                _context.CoverLetterExperiences.RemoveRange(existing.CoverLetterExperiences);
                existing.CoverLetterExperiences = coverLetter.CoverLetterExperiences;

                _context.CoverLetterSkills.RemoveRange(existing.CoverLetterSkills);
                existing.CoverLetterSkills = coverLetter.CoverLetterSkills;

                _context.CoverLetterLanguages.RemoveRange(existing.CoverLetterLanguages);
                existing.CoverLetterLanguages = coverLetter.CoverLetterLanguages;
            }
            else
            {
                coverLetter.UserId = userId;
                _context.CoverLetters.Add(coverLetter);
            }
            await _context.SaveChangesAsync();
        }
        private void EnsureRequiredFieldsAreFilled(CoverLetter coverLetter)
        {
            foreach (var e in coverLetter.CoverLetterExperiences ?? new())
            {
                e.JobTitle = e.JobTitle?.Trim() ?? string.Empty;
                e.CompanyName = e.CompanyName?.Trim() ?? string.Empty;
                e.CompanyLocation = e.CompanyLocation?.Trim() ?? string.Empty;
                e.StartDate = e.StartDate?.Trim() ?? string.Empty;
                e.EndDate = e.EndDate?.Trim() ?? string.Empty;
                e.EmploymentType = e.EmploymentType?.Trim() ?? string.Empty;
                e.Description = e.Description?.Trim() ?? string.Empty;
            }

            foreach (var s in coverLetter.CoverLetterSkills ?? new())
            {
                s.SkillName = s.SkillName?.Trim() ?? string.Empty;
                s.SkillCategory = s.SkillCategory?.Trim() ?? string.Empty;
                s.SkillDescription = s.SkillDescription?.Trim() ?? string.Empty;
            }

            foreach (var l in coverLetter.CoverLetterLanguages ?? new())
            {
                l.LanguageName = l.LanguageName?.Trim() ?? string.Empty;
                l.ProficiencyLevel = l.ProficiencyLevel?.Trim() ?? string.Empty;
            }

            coverLetter.FirstName = coverLetter.FirstName?.Trim() ?? string.Empty;
            coverLetter.SecondName = coverLetter.SecondName?.Trim() ?? string.Empty;
            coverLetter.ThirdName = coverLetter.ThirdName?.Trim() ?? string.Empty;
            coverLetter.Email = coverLetter.Email?.Trim() ?? string.Empty;
            coverLetter.PhoneNumber = coverLetter.PhoneNumber?.Trim() ?? string.Empty;
            coverLetter.Address = coverLetter.Address?.Trim() ?? string.Empty;
            coverLetter.JobTitle = coverLetter.JobTitle?.Trim() ?? string.Empty;
            coverLetter.Summary = coverLetter.Summary?.Trim() ?? string.Empty;
            coverLetter.RecipientFullName = coverLetter.RecipientFullName?.Trim() ?? string.Empty;
            coverLetter.RecipientAddress = coverLetter.RecipientAddress?.Trim() ?? string.Empty;
            coverLetter.Introduction = coverLetter.Introduction?.Trim() ?? string.Empty;
            coverLetter.BodyContent = coverLetter.BodyContent?.Trim() ?? string.Empty;
            coverLetter.Closing = coverLetter.Closing?.Trim() ?? string.Empty;
            coverLetter.SignatureName = coverLetter.SignatureName?.Trim() ?? string.Empty;
        }
    }
}
