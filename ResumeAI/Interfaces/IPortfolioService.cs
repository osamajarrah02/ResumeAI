using ResumeAI.DTOs;
using ResumeAI.Models.Portfolio;
using ResumeAI.Models.Resume;

namespace ResumeAI.Interfaces
{
    public interface IPortfolioService
    {
        Task<PortfolioDTO> GetPortfolioByUserIdAsync(string userId);
        Task CreatePortfolioAsync(PortfolioDTO portfolioDTO, string userId);
        Task UpdatePortfolioAsync(PortfolioDTO portfolioDTO);
        Task SaveGeneratedPortFolioAsync(string userId, Portfolio portfolio);
    }
}
