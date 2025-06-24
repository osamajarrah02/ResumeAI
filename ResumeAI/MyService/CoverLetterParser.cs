using System.Text.Json;
using Microsoft.SemanticKernel;
using ResumeAI.Interfaces;
using ResumeAI.Models.CoverLetter;

namespace ResumeAI.MyService
{
    public class CoverLetterParser : ICoverLetterParser
    {
        Kernel _kernel;

        public CoverLetterParser(Kernel kernel)
        {
            _kernel = kernel;
        }
        public async Task<CoverLetter> ParseCoverLetterAsync(string rawText)
        {
            var prompt =
            @"
                You are a professional cover letter writer.

                Extract the following fields from the given resume text and return them as a JSON object only. Do not include any explanations or commentary. Output ONLY valid JSON.

                Please include:
                - A proper salutation to the recipient.
                - An engaging introduction paragraph.
                - A compelling body that highlights experience, skills, and accomplishments.
                - A strong closing paragraph.
                - A professional sign-off with the full name.

                ## Personal Information:
                - First Name: {FirstName}
                - Second Name: {SecondName}
                - Third Name: {ThirdName}
                - Email: {Email}
                - Phone Number: {PhoneNumber}
                - Address: {Address}
                - Job Title: {JobTitle}
                - Summary: {Summary}

                ## Recipient Information:
                - Full Name: {RecipientFullName}
                - Address: {RecipientAddress}

                ## Cover Letter Sections:
                - Introduction: {Introduction}
                - Body Content: {BodyContent}
                - Closing Statement: {Closing}
                - Signature Name: {SignatureName}

                ## Relevant Experiences:
                {List of Experiences:
                  - Job Title: {JobTitle}
                  - Company: {CompanyName}
                  - Location: {CompanyLocation}
                  - Duration: {StartDate} to {EndDate}
                  - Type: {EmploymentType}
                  - Description: {Description}
                }

                ## Skills:
                {List of Skills:
                  - Skill Name: {SkillName}
                  - Category: {SkillCategory}
                  - Description: {SkillDescription}
                }

                ## Languages:
                {List of Languages:
                  - Language: {LanguageName}
                  - Proficiency: {ProficiencyLevel}
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

            var coverLetter = JsonSerializer.Deserialize<CoverLetter>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return coverLetter!;
        }
    }
}
