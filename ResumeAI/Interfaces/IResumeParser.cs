using ResumeAI.Models.Resume;

namespace ResumeAI.Interfaces
{
    public interface IResumeParser
    {
        Task<Resume> ParseResumeAsync(string rawText);
    }
}