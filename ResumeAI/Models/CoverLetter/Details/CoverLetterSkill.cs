namespace ResumeAI.Models.CoverLetter.Details
{
    public class CoverLetterSkill
    {
        public int Id { get; set; }
        public string SkillName { get; set; }
        public string SkillCategory { get; set; }
        public string SkillDescription { get; set; }
        public int CoverLetterId { get; set; }
        public CoverLetter CoverLetter { get; set; }
    }
}
