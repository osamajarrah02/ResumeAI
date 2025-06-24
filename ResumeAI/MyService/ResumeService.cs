using HTU_FinalProject.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ResumeAI.Data;
using ResumeAI.DTOs;
using ResumeAI.Models.Person;
using ResumeAI.Models.Resume;

public class ResumeService : IResume
{
    private readonly UserManager<Person> _userManager;
    private readonly ApplicationDbContext _context;

    public ResumeService(UserManager<Person> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<ResumeDTO?> GetResumeAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return null;

        return new ResumeDTO
        {
            PersonFirstName = user.PersonFirstName,
            PersonLastName = user.PersonLastName,
            ImageBase64 = user.ProfileImage != null
                ? Convert.ToBase64String(user.ProfileImage)
                : null,
            Points = user.Points.ToString()
        };
    }

    public async Task<bool> UpdateProfileImageAsync(string userId, IFormFile imageFile)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        if (imageFile != null && imageFile.Length > 0)
        {
            using var ms = new MemoryStream();
            await imageFile.CopyToAsync(ms);
            user.ProfileImage = ms.ToArray();
        }

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task SaveGeneratedResumeAsync(string userId, Resume resume)
    {
        EnsureRequiredFieldsAreFilled(resume);

        resume.UserId = userId;
        resume.ResumeCreatedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");
        resume.ResumeModifiedDate = DateTime.UtcNow.ToString("yyyy-MM-dd");

        _context.Resumes.Add(resume);
        await _context.SaveChangesAsync();
    }
    public async Task<Resume?> GetResumeModelAsync(string userId)
    {
        return await _context.Resumes
            .Include(r => r.Certificates)
            .Include(r => r.Educations)
            .Include(r => r.Experiences)
            .Include(r => r.Skills)
            .Include(r => r.Languages)
            .Include(r => r.Links)
            .FirstOrDefaultAsync(r => r.UserId == userId && !r.IsDeleted);
    }
    private void EnsureRequiredFieldsAreFilled(Resume resume)
    {

        foreach (var cert in resume.Certificates ?? new())
        {
            cert.CertificateType = cert.CertificateType?.Trim();
            cert.CourseName = cert.CourseName?.Trim();
            cert.InstitutionName = cert.InstitutionName?.Trim();
            cert.StartDate = cert.StartDate?.Trim();
            cert.EndDate = cert.EndDate?.Trim();
            cert.GPA = cert.GPA?.Trim();
        }

        foreach (var edu in resume.Educations ?? new())
        {
            edu.Degree = edu.Degree?.Trim();
            edu.InstitutionName = edu.InstitutionName?.Trim();
            edu.FieldOfStudy = edu.FieldOfStudy?.Trim();
            edu.GPA = edu.GPA?.Trim();
            edu.Description = edu.Description?.Trim();
            edu.StartDate = edu.StartDate?.Trim();
            edu.EndDate = edu.EndDate?.Trim();
        }

        foreach (var skill in resume.Skills ?? new())
        {
            skill.SkillName = skill.SkillName?.Trim();
            skill.SkillCategory = skill.SkillCategory?.Trim();
            skill.SkillDescription = skill.SkillDescription?.Trim();
        }

        foreach (var lang in resume.Languages ?? new())
        {
            lang.LanguageName = lang.LanguageName?.Trim();
            lang.ProficiencyLevel = lang.ProficiencyLevel?.Trim();
        }

        foreach (var link in resume.Links ?? new())
        {
            link.LinkName = link.LinkName?.Trim();
            link.LinkUrl = link.LinkUrl?.Trim();
        }

        foreach (var exp in resume.Experiences ?? new())
        {
            exp.JobTitle = exp.JobTitle?.Trim();
            exp.CompanyName = exp.CompanyName?.Trim();
            exp.CompanyLocation = exp.CompanyLocation?.Trim();
            exp.EmploymentType = exp.EmploymentType?.Trim();
            exp.Description = exp.Description?.Trim();
            exp.StartDate = exp.StartDate?.Trim();
            exp.EndDate = exp.EndDate?.Trim();
        }

        resume.FirstName = resume.FirstName?.Trim();
        resume.SecondName = resume.SecondName?.Trim();
        resume.ThirdName = resume.ThirdName?.Trim();
        resume.Email = resume.Email?.Trim();
        resume.PhoneNumber = resume.PhoneNumber?.Trim();
        resume.Address = resume.Address?.Trim();
        resume.JobTitle = resume.JobTitle?.Trim();
        resume.DateOfBirth = resume.DateOfBirth?.Trim();
        resume.Summary = resume.Summary?.Trim();
    }
    public async Task<bool> UpdateResumeAsync(Resume resume)
    {
        _context.Resumes.Update(resume);
        return await _context.SaveChangesAsync() > 0;
    }
}