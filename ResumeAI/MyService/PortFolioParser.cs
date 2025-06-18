using System.Text.Json;
using Microsoft.SemanticKernel;
using ResumeAI.Interfaces;
using ResumeAI.Models.Portfolio;
using ResumeAI.Models.Resume;

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
            You are a portfolio parsing AI.

            Extract all possible personal and professional information from the input below and return it in a structured JSON object with the following fields:

            Return this exact format:
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
            try
            {
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
            catch (JsonException jex)
            {
                Console.WriteLine("JSON parsing failed: " + jex.Message);
                throw new ApplicationException("Failed to parse resume JSON output from AI.", jex);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Resume parsing failed: " + ex.Message);
                throw new ApplicationException("An unexpected error occurred while parsing resume.", ex);
            }
        }
    }
}
