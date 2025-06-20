using ResumeAI.Models.CoverLetter.Details;
using ResumeAI.Models.Person;
using ResumeAI.Models.Resume.Details;

namespace ResumeAI.Models.CoverLetter
{
    public class CoverLetter : PersonalInfo
    {
        public int Id { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientAddress { get; set; }
        public DateTime Date { get; set; }
        public string Introduction { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string BodyContent { get; set; }
        public string Closing { get; set; }
        public string SignatureName { get; set; }
        public List<CoverLetterExperience> CoverLetterExperiences { get; set; } = new();
        public List<CoverLetterSkill> CoverLetterSkills { get; set; } = new();
        public List<CoverLetterLanguage> CoverLetterLanguages { get; set; } = new();
    }
}