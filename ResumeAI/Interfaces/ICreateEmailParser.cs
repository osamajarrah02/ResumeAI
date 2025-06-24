namespace ResumeAI.Interfaces
{
    public interface ICreateEmailParser
    {
        Task<string> ParseEmailAsync(string rawText);
    }
}
