using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.DTOs.Details;
using ResumeAI.Interfaces;
using ResumeAI.Models.Portfolio;
using ResumeAI.Models.Resume;

namespace ResumeAI.MyService
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;

        public PortfolioService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PortfolioDTO?> GetPortfolioByUserIdAsync(string userId)
        {
            var model = await _context.Portfolios
                .Include(f => f.Services)
                .Include(f => f.Projects)
                .FirstOrDefaultAsync(f => f.UserId == userId && !f.IsDeleted);

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
                UserId = model.UserId,
                PortFolioCreatedDate = DateTime.TryParse(model.CreatedDate, out var created) ? created : null,
                PortFolioModifiedDate = DateTime.TryParse(model.ModifiedDate, out var modified) ? modified : null,
                PersonalImage = model.PersonalImage,
                Services = model.Services?.Select(s => new ServiceDTO
                {
                    Id = s.Id,
                    ServiceName = s.ServiceName,
                    ServiceDescription = s.ServiceDescription,
                    ServiceImage = s.ServiceImage
                }).ToList(),
                Projects = model.Projects?.Select(p => new ProjectDTO
                {
                    ProjectName = p.ProjectName,
                    ProjectDescription = p.ProjectDescription,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    ProjectAttachments = p.ProjectAttachments,
                    ProjectLink = p.ProjectLink
                }).ToList()
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
        public Task UpdatePortfolioAsync(PortfolioDTO portfolioDTO)
        {
            throw new NotImplementedException();
        }
        public async Task SaveGeneratedPortFolioAsync(string userId, Portfolio portfolio)
        {
            EnsureRequiredFieldsAreFilled(portfolio);
            var existing = await _context.Portfolios
                .Include(f => f.Services)
                .Include(f => f.Projects)
                .FirstOrDefaultAsync(f => f.UserId == userId && !f.IsDeleted);


            if (existing != null)
            {
                existing.FirstName = portfolio.FirstName;
                existing.SecondName = portfolio.SecondName;
                existing.ThirdName = portfolio.ThirdName;
                existing.Email = portfolio.Email;
                existing.PhoneNumber = portfolio.PhoneNumber;
                existing.Address = portfolio.Address;
                existing.JobTitle = portfolio.JobTitle;
                existing.DateOfBirth = portfolio.DateOfBirth;
                existing.Summary = portfolio.Summary;
                existing.PersonalImage = portfolio.PersonalImage;
                _context.Services.RemoveRange(existing.Services);
                existing.Services = portfolio.Services;

                _context.Projects.RemoveRange(existing.Projects);
                existing.Projects = portfolio.Projects;
            }
            else
            {
                portfolio.UserId = userId;
                portfolio.CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
                _context.Portfolios.Add(portfolio);
            }
            portfolio.ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
            await _context.SaveChangesAsync();
        }
        private void EnsureRequiredFieldsAreFilled(Portfolio portfolio)
        {
            foreach (var s in portfolio.Services ?? new List<ResumeAI.Models.Portfolio.Service>())
            {
                s.ServiceName = s.ServiceName?.Trim();
                s.ServiceDescription = s.ServiceDescription?.Trim();
            }

            foreach (var p in portfolio.Projects ?? new List<Project>())
            {
                p.ProjectName = p.ProjectName?.Trim();
                p.ProjectDescription = p.ProjectDescription?.Trim();
                p.ProjectAttachments = p.ProjectAttachments?.Trim();
                p.ProjectLink = p.ProjectLink?.Trim();
            }

            portfolio.FirstName = portfolio.FirstName?.Trim();
            portfolio.SecondName = portfolio.SecondName?.Trim();
            portfolio.ThirdName = portfolio.ThirdName?.Trim();
            portfolio.Email = portfolio.Email?.Trim();
            portfolio.PhoneNumber = portfolio.PhoneNumber?.Trim();
            portfolio.Address = portfolio.Address?.Trim();
            portfolio.JobTitle = portfolio.JobTitle?.Trim();
            portfolio.DateOfBirth = portfolio.DateOfBirth?.Trim();
            portfolio.Summary = portfolio.Summary?.Trim();
        }
    }
}
