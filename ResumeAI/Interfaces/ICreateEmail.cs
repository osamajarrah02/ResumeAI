using ResumeAI.DTOs;

namespace ResumeAI.Interfaces
{
    public interface ICreateEmail
    {
        Task CreateEmailAsync(CreateEmailDTO createEmailDTO, string userId);
    }
}
