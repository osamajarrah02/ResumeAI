namespace ResumeAI.Models.Resume.Details
{
    public class Education
    {
        public int Id { get; set; }
        public string InstitutionName { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string GPA { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }
}
