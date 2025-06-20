using ResumeAI.Models.Person;
using ResumeAI.Models.Resume.Details;

namespace ResumeAI.Models.Portfolio
{
    public class Portfolio : PersonalInfo
    {
        public int Id { get; set; }
        public byte[]? PersonalImage { get; set; }
        public List<Service> Services { get; set; }
        public List<Project> Projects { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string? CreatedDate { get; set; }
        public string? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
