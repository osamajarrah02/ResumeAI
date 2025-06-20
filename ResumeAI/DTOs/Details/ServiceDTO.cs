namespace ResumeAI.DTOs.Details
{
    public class ServiceDTO
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public byte[] ServiceImage { get; set; }
        public IFormFile? ServiceImageFile { get; set; }
    }
}
