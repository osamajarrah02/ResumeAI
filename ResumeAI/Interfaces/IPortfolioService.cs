using ResumeAI.DTOs;
using ResumeAI.Models.Portfolio;

public interface IPortfolioService
{
    Task<PortfolioDTO?> GetPortFolioAsync(string userId);
    Task<Portfolio?> GetPortFolioModelAsync(string userId);
    Task<bool> UpdatePortfolioAsync(Portfolio portfolio);
    Task SaveGeneratedPortFolioAsync(string userId, Portfolio portfolio);
}
