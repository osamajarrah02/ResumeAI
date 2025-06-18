using ResumeAI.DTOs;

namespace ResumeAI.Interfaces
{
    public interface IPortfolioService
    {
        Task<PortfolioDTO> GetPortfolioByUserIdAsync(string userId);
        Task CreatePortfolioAsync(PortfolioDTO portfolioDTO, string userId);
        Task UpdatePortfolioAsync(PortfolioDTO portfolioDTO);
    }
}
