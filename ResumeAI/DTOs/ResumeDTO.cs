using ResumeAI.DTOs.Details;

namespace ResumeAI.DTOs
{
    public class ResumeDTO
    {
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string Points { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Summary { get; set; }
        public string? Address { get; set; }
        public string? DateOfBirth { get; set; }
        public string JobTitle { get; set; }
        public string Id { get; set; }
        public DateTime? ResumeCreatedDate { get; set; }
        public DateTime? ResumeModifiedDate { get; set; }

        public IFormFile? ImageFile { get; set; }
        public string? ImageBase64 { get; set; }
        public IFormFile? ResumeImage { get; set; }

        public List<CertificateDTO> Certificates { get; set; }
        public List<EducationDTO> Educations { get; set; }
        public List<ExperienceDTO> Experiences { get; set; }
        public List<LanguageDTO> Languages { get; set; }
        public List<LinkDTO> Links { get; set; }
        public List<SkillDTO> Skills { get; set; }

        public ResumeDTO()
        {
            Certificates = new List<CertificateDTO> { new CertificateDTO() };
            Educations = new List<EducationDTO> { new EducationDTO() };
            Experiences = new List<ExperienceDTO> { new ExperienceDTO() };
            Languages = new List<LanguageDTO> { new LanguageDTO() };
            Links = new List<LinkDTO> { new LinkDTO() };
            Skills = new List<SkillDTO> { new SkillDTO() };
        }

        public string BuildCombinedPersonalInfo()
        {
            var parts = new string?[]
            {
                FirstName,
                SecondName,
                LastName,
                Title,
                Email,
                PhoneNumber,
                Summary,
                Address,
                DateOfBirth,
                JobTitle,
                Id,
            };
            return string.Join(" | ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
        }
    }
}
