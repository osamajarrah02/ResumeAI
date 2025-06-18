using Microsoft.AspNetCore.Identity;

namespace ResumeAI.Models.Person
{
    public class Person : IdentityUser
    {
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public byte[]? ProfileImage { get; set; }
        public int Points { get; set; } = 0;
    }
}
