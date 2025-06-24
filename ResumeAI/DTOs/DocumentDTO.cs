using ResumeAI.Models.CoverLetter;
using ResumeAI.Models.Portfolio;
using ResumeAI.Models.Resume;

namespace ResumeAI.DTOs
{
    public class DocumentDTO
    {
        public string FullName { get; set; }
        public List<ResumeDTO> Resumes { get; set; }
        public List<PortfolioDTO> Portfolios { get; set; }
        public List<CoverLetterDTO> CoverLetters { get; set; }
    }
}
