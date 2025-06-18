namespace ResumeAI.Models.Person
{
    public class User : Person
    {
        public List<ResumeAI.Models.Resume.Resume> Resumes { get; set; }
        public List<ResumeAI.Models.Portfolio.Portfolio> Portfolios { get; set; }
    }
}
