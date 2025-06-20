namespace ResumeAI.Models.CoverLetter.Details
{
    public class CoverLetterLanguage
    {
        public int Id { get; set; }
        public string LanguageName { get; set; }
        public string ProficiencyLevel { get; set; }
        public int CoverLetterId { get; set; }
        public CoverLetter CoverLetter { get; set; }
    }
}
