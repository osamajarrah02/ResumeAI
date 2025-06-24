using ResumeAI.DTOs.Details;

namespace ResumeAI.DTOs
{
    public class CoverLetterDTO
    {
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string ThirdName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string Id { get; set; }
        public string JobTitle { get; set; }
        public string Summary { get; set; }
        public string RecipientFullName { get; set; }
        public string RecipientAddress { get; set; }
        public DateTime Date { get; set; }
        public string Introduction { get; set; }
        public string BodyContent { get; set; }
        public string Closing { get; set; }
        public string SignatureName { get; set; }
        public List<CoverLetterExperienceDTO> CoverLetterExperiences { get; set; }
        public List<CoverLetterSkillDTO> CoverLetterSkills { get; set; }
        public List<CoverLetterLanguageDTO> CoverLetterLanguages { get; set; }
        public CoverLetterDTO()
        {
            CoverLetterExperiences = new List<CoverLetterExperienceDTO> { new CoverLetterExperienceDTO() };
            CoverLetterSkills = new List<CoverLetterSkillDTO> { new CoverLetterSkillDTO() };
            CoverLetterLanguages = new List<CoverLetterLanguageDTO> { new CoverLetterLanguageDTO() };
        }
    }
}
