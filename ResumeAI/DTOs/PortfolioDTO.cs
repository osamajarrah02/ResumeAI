using ResumeAI.DTOs.Details;
using ResumeAI.Models.Person;

namespace ResumeAI.DTOs
{
    public class PortfolioDTO
    {
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string ThirdName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string JobTitle { get; set; }
        public string? DateOfBirth { get; set; }
        public string Summary { get; set; }
        public string Id { get; set; }

        public DateTime? PortFolioCreatedDate { get; set; }
        public DateTime? PortFolioModifiedDate { get; set; }

        public string? ImageBase64 { get; set; }
        public IFormFile? PortFolioImage { get; set; }

        public List<ServiceDTO> Services { get; set; }
        public List<ProjectDTO> Projects { get; set; }
        public PortfolioDTO()
        {
            Services = new List<ServiceDTO>();
            Projects = new List<ProjectDTO>();
        }

        public string BuildCombinedPersonalInfo()
        {
            var parts = new string?[]
            {
                FirstName,
                SecondName,
                ThirdName,
                JobTitle,
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
