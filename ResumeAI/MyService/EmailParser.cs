using Microsoft.SemanticKernel;
using ResumeAI.Interfaces;

namespace ResumeAI.MyService
{
    public class EmailParser : ICreateEmailParser
    {
        private readonly Kernel _kernel;

        public EmailParser(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<string> ParseEmailAsync(string rawText)
        {
            var prompt = @"
            You are a helpful email assistant.

            Your task is:
            1. Understand the user's message.
            2. If they want to write an email, try to extract these fields:
               - EmailType
               - Subject
               - RecipientName
               - SenderName
               - Tone
               - Purpose
               - AdditionalInfo

            If some fields are missing, ask them clearly and politely one by one:
            - Example: ""What is the subject of the email?""
            Only ask what's still missing.

            Once you have all the fields, generate a complete email message using them.

            Do NOT return JSON or code. Just return plain text:
            - Ask questions if needed.
            - Or generate a professional email if you have everything.

            User Message:
            {{ $input }}

            Response:
            ";

            var extractFunction = _kernel.CreateFunctionFromPrompt(prompt);

            var result = await _kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = rawText
            });

            return result.ToString();
        }
    }
}
