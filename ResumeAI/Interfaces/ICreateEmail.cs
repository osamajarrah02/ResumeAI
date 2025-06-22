using ResumeAI.DTOs;
using ResumeAI.Models.Email;

namespace ResumeAI.Interfaces
{
    public interface ICreateEmail
    {
        Task<CreateEmailDTO> GetEmailByUserIdAsync(string userId);
        Task CreateEmailAsync(CreateEmailDTO createEmailDTO, string userId);
    }
}
