using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Models.Person;
using ResumeAI.Models.Portfolio;
using ResumeAI.Interfaces;

public class PortfolioService : IPortfolioService
{
    private readonly UserManager<Person> _userManager;
    private readonly ApplicationDbContext _context;

    public PortfolioService(UserManager<Person> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<PortfolioDTO?> GetPortFolioAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        return new PortfolioDTO
        {
            PersonFirstName = user.PersonFirstName,
            PersonLastName = user.PersonLastName,
        };
    }

    public async Task<Portfolio?> GetPortFolioModelAsync(string userId)
    {
        return await _context.Portfolios
            .Include(p => p.Services)
            .Include(p => p.Projects)
            .FirstOrDefaultAsync(p => p.UserId == userId && !p.IsDeleted);
    }

    public async Task SaveGeneratedPortFolioAsync(string userId, Portfolio portfolio)
    {
        EnsureRequiredFieldsAreFilled(portfolio);

        portfolio.UserId = userId;
        portfolio.CreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        portfolio.ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

        _context.Portfolios.Add(portfolio);
        await _context.SaveChangesAsync();
    }

    private void EnsureRequiredFieldsAreFilled(Portfolio portfolio)
    {
        foreach (var service in portfolio.Services ?? new())
        {
            service.ServiceName = service.ServiceName?.Trim();
            service.ServiceDescription = service.ServiceDescription?.Trim();
        }

        foreach (var project in portfolio.Projects ?? new())
        {
            project.ProjectName = project.ProjectName?.Trim();
            project.ProjectDescription = project.ProjectDescription?.Trim();
            project.ProjectAttachments = project.ProjectAttachments?.Trim();
            project.ProjectLink = project.ProjectLink?.Trim();
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
    public async Task<bool> UpdatePortfolioAsync(Portfolio portfolio)
    {
        var existing = await _context.Portfolios
        .Include(p => p.Services)
        .Include(p => p.Projects)
        .FirstOrDefaultAsync(p => p.UserId == portfolio.UserId && !p.IsDeleted);

        if (existing == null) return false;

        // 🔄 Update fields
        existing.FirstName = portfolio.FirstName;
        existing.SecondName = portfolio.SecondName;
        existing.ThirdName = portfolio.ThirdName;
        existing.Email = portfolio.Email;
        existing.PhoneNumber = portfolio.PhoneNumber;
        existing.Address = portfolio.Address;
        existing.JobTitle = portfolio.JobTitle;
        existing.DateOfBirth = portfolio.DateOfBirth;
        existing.Summary = portfolio.Summary;
        existing.ModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

        if (portfolio.PersonalImage != null)
            existing.PersonalImage = portfolio.PersonalImage;

        // 🔁 Replace child collections
        existing.Services = portfolio.Services ?? new();
        existing.Projects = portfolio.Projects ?? new();

        await _context.SaveChangesAsync();
        return true;
    }
}
