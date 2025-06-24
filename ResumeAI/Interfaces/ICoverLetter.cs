using ResumeAI.DTOs;
using ResumeAI.Models.CoverLetter;

namespace ResumeAI.Interfaces
{
    public interface ICoverLetter
    {
        Task<CoverLetterDTO?> GetCoverLetterAsync(string userId);
        Task SaveGeneratedCoverLetterAsync(string userId, CoverLetter coverLetter);
        Task<CoverLetter?> GetCoverLetterModelAsync(string userId);
        Task<bool> UpdateCoverLetterAsync(CoverLetter updated);
    }
}