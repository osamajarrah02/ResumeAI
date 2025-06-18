using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.DTOs.Details;
using ResumeAI.Interfaces;
using ResumeAI.Models.Portfolio;

namespace ResumeAI.MyService
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PortfolioDTO> GetPortfolioByUserIdAsync(string userId)
        {
            var model = await _context.Portfolios
                .Include(p => p.Projects)
                .Include(p => p.Services)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (model == null) return null;

            return new PortfolioDTO
            {
                FirstName = model.FirstName,
                SecondName = model.SecondName,
                ThirdName = model.ThirdName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                JobTitle = model.JobTitle,
                DateOfBirth = model.DateOfBirth,
                Summary = model.Summary,
                Services = model.Services?.Select(s => new ServiceDTO
                {
                    ServiceName = s.ServiceName,
                    ServiceDescription = s.ServiceDescription,
                }).ToList(),
                Projects = model.Projects?.Select(p => new ProjectDTO
                {
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    ProjectAttachments = p.ProjectAttachments,
                    ProjectLink = p.ProjectLink,
                }).ToList(),
                UserId = model.UserId
            };
        }

        public async Task CreatePortfolioAsync(PortfolioDTO portfolioDTO, string userId)
        {
            var portfolio = new Portfolio
            {
                UserId = userId,
                FirstName = portfolioDTO.FirstName,
                SecondName = portfolioDTO.SecondName,
                ThirdName = portfolioDTO.ThirdName,
                Email = portfolioDTO.Email,
                PhoneNumber = portfolioDTO.PhoneNumber,
                Address = portfolioDTO.Address,
                Summary = portfolioDTO.Summary,
                DateOfBirth = portfolioDTO.DateOfBirth,
                JobTitle = portfolioDTO.JobTitle,
                Services = portfolioDTO.Services?.Select(s => new ResumeAI.Models.Portfolio.Service
                {
                    ServiceName = s.ServiceName,
                    ServiceDescription = s.ServiceDescription
                }).ToList(),
                Projects = portfolioDTO.Projects?.Select(p => new Project
                {
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    ProjectAttachments = p.ProjectAttachments,
                    ProjectLink = p.ProjectLink,
                }).ToList(),
            };

            _context.Portfolios.Add(portfolio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePortfolioAsync(PortfolioDTO portfolioDTO)
        {
            
        }
    }
}
