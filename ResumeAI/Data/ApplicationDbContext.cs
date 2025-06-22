using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ResumeAI.Models.CoverLetter;
using ResumeAI.Models.CoverLetter.Details;
using ResumeAI.Models.Email;
using ResumeAI.Models.Person;
using ResumeAI.Models.Portfolio;
using ResumeAI.Models.Resume;
using ResumeAI.Models.Resume.Details;

namespace ResumeAI.Data
{
    public class ApplicationDbContext : IdentityDbContext<Person>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Link> Links { get; set; }
        public DbSet<Portfolio> Portfolios { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<CreateEmail> CreateEmails { get; set; }
        public DbSet<CoverLetter> CoverLetters { get; set; }
        public DbSet<CoverLetterExperience> CoverLetterExperiences { get; set; }
        public DbSet<CoverLetterSkill> CoverLetterSkills { get; set; }
        public DbSet<CoverLetterLanguage> CoverLetterLanguages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure discriminator for Person inheritance
            builder.Entity<Person>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Person>("Person")
                .HasValue<User>("User")
                .HasValue<Admin>("Admin");
        }
    }
}