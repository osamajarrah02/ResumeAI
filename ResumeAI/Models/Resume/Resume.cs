using ResumeAI.Models.Person;
using ResumeAI.Models.Resume.Details;

namespace ResumeAI.Models.Resume
{
    public class Resume : PersonalInfo
    {
        public int Id { get; set; }
        public string? ResumeCreatedDate { get; set; }
        public string? ResumeModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Certificate> Certificates { get; set; }
        public List<Education> Educations { get; set; }
        public List<Experience> Experiences { get; set; }
        public List<Skill> Skills { get; set; }
        public List<Link> Links { get; set; }
        public List<Language> Languages { get; set; }
    }
}
