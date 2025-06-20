using System.Text.Json;
using Microsoft.AspNetCore.Routing;
using Microsoft.SemanticKernel;
using ResumeAI.Interfaces;
using ResumeAI.Models.Email;
using ResumeAI.Models.Portfolio;

namespace ResumeAI.MyService
{
    public class EmailParser : ICreateEmailParser
    {
        Kernel _kernel;
        public EmailParser(Kernel kernel)
        {
            _kernel = kernel;
        }
        public async Task<CreateEmail> ParseEmailAsync(string rawText)
        {
            var prompt = @"
            You are a professional email writing assistant.

            From the given text, extract the following fields and return them as a JSON object. Do NOT include explanations, commentary, or extra formatting. Output ONLY valid JSON.

            If any field is unknown, use an empty string or null.

            Extract and return this format:
            {
              ""Subject"": ""..."",
              ""RecipientName"": ""..."",
              ""SenderName"": ""..."",
              ""Tone"": ""..."",  // Formal, Friendly, Semi-formal
              ""Purpose"": ""..."",
              ""AdditionalInfo"": ""..."",
              ""PersonFirstName"": ""..."",
              ""PersonLastName"": ""...""
            }

            PORTFOLIO OR RESUME TEXT:
            {{ $input }}

            JSON:
            ";
            var extractFunction = _kernel.CreateFunctionFromPrompt(prompt);

            var result = await _kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = rawText
            });
            var json = result.ToString();

            var email = JsonSerializer.Deserialize<CreateEmail>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return email!;
        }
    }
}