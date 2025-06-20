using System.Text.Json;
using Microsoft.SemanticKernel;
using ResumeAI.Interfaces;
using ResumeAI.Models.Resume;

namespace ResumeAI.MyService
{
    public class ResumeParser : IResumeParser
    {
        Kernel _kernel;

        public ResumeParser(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<Resume> ParseResumeAsync(string rawText)
        {
            var prompt = @"
            You are a resume extraction AI.

            Extract the following fields from the given resume text and return them as a JSON object only. Do not include any explanations or commentary. Output ONLY valid JSON.

            Return this format:

            {
              ""FirstName"": ""..."",
              ""SecondName"": ""..."",
              ""ThirdName"": ""..."",
              ""Email"": ""..."",
              ""PhoneNumber"": ""..."",
              ""Address"": ""..."",
              ""JobTitle"": ""..."",
              ""DateOfBirth"": ""..."",
              ""Summary"": ""..."",
              ""Educations"": [ { ... } ],
              ""Experiences"": [ { ... } ],
              ""Certificates"": [ { ... } ],
              ""Skills"": [ { ... } ],
              ""Languages"": [ { ... } ],
              ""Links"": [ { ... } ]
            }

            If any field is missing or unknown, do the following:
            - For string fields, leave them empty or null.
            - For boolean fields (like IsCurrent), use **false**.

            CV TEXT:
            {{$input}}

            JSON:
            ";
            var extractFunction = _kernel.CreateFunctionFromPrompt(prompt);
            var result = await _kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = rawText
            });
            var json = result.ToString();

            var resume = JsonSerializer.Deserialize<Resume>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return resume!;
        }
    }
}
