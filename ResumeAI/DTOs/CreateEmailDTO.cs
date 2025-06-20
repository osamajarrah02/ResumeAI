using ResumeAI.Models.Person;

namespace ResumeAI.DTOs
{
    public class CreateEmailDTO
    {
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string EmailType { get; set; }
        public string Subject { get; set; }
        public string RecipientName { get; set; }
        public string SenderName { get; set; }
        public string Tone { get; set; }
        public string Purpose { get; set; }
        public string AdditionalInfo { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
    }
}