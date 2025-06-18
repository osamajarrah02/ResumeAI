using ResumeAI.Models.Portfolio;
using ResumeAI.Models.Resume;

namespace ResumeAI.Interfaces
{
    public interface IPortfolioParser
    {
        Task<Portfolio> ParsePortFolioAsync(string rawText);
    }
}
