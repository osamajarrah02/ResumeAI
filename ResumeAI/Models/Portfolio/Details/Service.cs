using ResumeAI.Models.Portfolio;

namespace ResumeAI.Models.Portfolio
{
    public class Service
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public byte[]? ServiceImage { get; set; }
        public int PortfolioId { get; set; }
        public Portfolio Portfolio { get; set; }
        public List<Project> Projects { get; set; }
    }
}
