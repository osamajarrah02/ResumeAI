namespace ResumeAI.Models.CoverLetter.Details
{
    public class CoverLetterExperience
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string CompanyLocation { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string? EmploymentType { get; set; }
        public string Description { get; set; }
        public int CoverLetterId { get; set; }
        public CoverLetter CoverLetter { get; set; }
    }
}
