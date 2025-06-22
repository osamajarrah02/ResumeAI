namespace ResumeAI.DTOs
{
    public class ProfileDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? ImageBase64 { get; set; }
        public int ResumeCount { get; set; }
        public int PortfolioCount { get; set; }
        public int CoverLetterCount { get; set; }
        public int EmailCount { get; set; }
    }
}