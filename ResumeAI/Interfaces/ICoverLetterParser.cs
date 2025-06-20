using ResumeAI.Models.CoverLetter;

namespace ResumeAI.Interfaces
{
    public interface ICoverLetterParser
    {
        Task<CoverLetter> ParseCoverLetterAsync(string rawText);
    }
}
