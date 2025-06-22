using ResumeAI.Models.Person;

namespace ResumeAI.DTOs
{
    public class AdminDashboardDTO
    {
        public List<User> Users { get; set; }
        public int ResumesCount { get; set; }
        public int PortfoliosCount { get; set; }
        public int CoverLettersCount { get; set; }
        public int EmailsCount { get; set; }
    }
}
