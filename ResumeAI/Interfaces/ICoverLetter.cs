using ResumeAI.DTOs;
using ResumeAI.Models.CoverLetter;
using ResumeAI.Models.Email;
using ResumeAI.Models.Resume;

namespace ResumeAI.Interfaces
{
    public interface ICoverLetter
    {
        Task<CoverLetterDTO?> GetCoverLetterAsync(string userId);
        Task SaveGeneratedCoverLetterAsync(string userId, CoverLetter coverLetter);
        Task<CoverLetter?> GetCoverLetterModelAsync(string userId);
    }
}