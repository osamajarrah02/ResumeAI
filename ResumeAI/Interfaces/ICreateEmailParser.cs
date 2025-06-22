using ResumeAI.Models.Email;
using ResumeAI.Models.Portfolio;

namespace ResumeAI.Interfaces
{
    public interface ICreateEmailParser
    {
        Task<string> ParseEmailAsync(string rawText);
    }
}
