using ResumeAI.DTOs;
using ResumeAI.Models.Resume;

namespace HTU_FinalProject.Interfaces
{
    public interface IResume
    {
        Task<ResumeDTO?> GetResumeAsync(string userId);
        Task<bool> UpdateProfileImageAsync(string userId, IFormFile imageFile);
        Task SaveGeneratedResumeAsync(string userId, Resume resume);
        Task<Resume?> GetResumeModelAsync(string userId);
        Task<bool> UpdateResumeAsync(Resume resume);
    }
}
