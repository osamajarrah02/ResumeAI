using System.Text.Json;
using Microsoft.SemanticKernel;
using ResumeAI.Interfaces;
using ResumeAI.Models.Portfolio;

namespace ResumeAI.MyService
{
    public class PortFolioParser : IPortfolioParser
    {
        private readonly Kernel _kernel;

        public PortFolioParser(Kernel kernel)
        {
            _kernel = kernel;
        }

        public async Task<Portfolio> ParsePortFolioAsync(string rawText)
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
              ""Services"": [
                {
                  ""ServiceName"": ""..."",
                  ""ServiceDescription"": ""...""
                }
              ],
              ""Projects"": [
                {
                  ""ProjectName"": ""..."",
                  ""ProjectDescription"": ""..."",
                  ""StartDate"": ""..."",
                  ""EndDate"": ""..."",
                  ""ProjectAttachments"": ""..."",
                  ""ProjectLink"": ""...""
                }
              ]
            }

            Guidelines:
            - If any field is unknown, use null or an empty string.
            - Dates can be rough approximations if not specified exactly.
            - Ensure that Services and Projects are clear and relevant.

            PORTFOLIO TEXT:
            {{$input}}

            JSON:
            ";
            var extractFunction = _kernel.CreateFunctionFromPrompt(prompt);

            var result = await _kernel.InvokeAsync(extractFunction, new()
            {
                ["input"] = rawText
            });
            var json = result.ToString();

            var portfolio = JsonSerializer.Deserialize<Portfolio>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return portfolio!;
        }
    }
}
