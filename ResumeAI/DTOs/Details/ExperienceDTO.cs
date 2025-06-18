namespace ResumeAI.DTOs.Details
{
    public class ExperienceDTO
    {
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsCurrent { get; set; }
        public string? EmploymentType { get; set; }
        public string Description { get; set; }
    }
}
