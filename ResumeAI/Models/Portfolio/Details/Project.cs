using ResumeAI.Models.Portfolio;

public class Project
{
    public int Id { get; set; }
    public string ProjectName { get; set; }
    public string ProjectDescription { get; set; }
    public string? StartDate { get; set; }
    public string? EndDate { get; set; }
    public string ProjectAttachments { get; set; }
    public string ProjectLink { get; set; }
    public int PortfolioId { get; set; }
    public Portfolio Portfolio { get; set; }
}