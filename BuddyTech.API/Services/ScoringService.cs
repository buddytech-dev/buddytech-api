using BuddyTech.API.Enums;
using BuddyTech.API.Models;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace BuddyTech.API.Services
{
    public class ScoringService : IScoringService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ScoringService> _logger;

        public ScoringService(HttpClient httpClient, ILogger<ScoringService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<LeadScore> CalculateScoreAsync(Lead lead)
        {
            var interactions = lead.Interactions?.Select(i => new
            {
                TypeOfContact = i.TypeOfContact.ToString(),
                InteractionContent = i.InteractionContent,
                InteractionDate = i.InteractionDate.ToString("yyyy-MM-dd")
            }).ToArray() ?? Array.Empty<object>();

            var payload = new
            {
                LeadId = lead.Id.ToString(),
                Title = lead.Title,
                Status = lead.Status.ToString(),
                Company = new
                {
                    RevenueRange = lead.Company.RevenueRange.ToString(),
                    Industry = lead.Company.Industry
                },
                Interactions = interactions
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/score", payload);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<AIResponseModel>();
                    if (result != null)
                    {
                        return new LeadScore
                        {
                            Score = result.Score,
                            UpdatedAt = DateOnly.FromDateTime(DateTime.Now)
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("IA offline: {Message}", ex.Message);
            }

            return new LeadScore { Score = 45, UpdatedAt = DateOnly.FromDateTime(DateTime.Now) };
        }

        public async Task<Suggestion> GenerateSuggestionAsync(Lead lead, int currentScore)
        {
            var interactions = lead.Interactions?.Select(i => new
            {
                TypeOfContact = i.TypeOfContact.ToString(),
                InteractionContent = i.InteractionContent,
                InteractionDate = i.InteractionDate.ToString("yyyy-MM-dd")
            }).ToArray() ?? Array.Empty<object>();

            var payload = new
            {
                LeadId = lead.Id.ToString(),
                Title = lead.Title,
                Status = lead.Status.ToString(),
                Company = new
                {
                    RevenueRange = lead.Company.RevenueRange.ToString(),
                    Industry = lead.Company.Industry
                },
                Interactions = interactions
            };

            try
            {
                var response = await _httpClient.PostAsJsonAsync("/api/score", payload);
                if (response.IsSuccessStatusCode)
                {
                    var ai = await response.Content.ReadFromJsonAsync<AIResponseModel>();
                    if (ai != null)
                    {
                        return new Suggestion
                        {
                            Notes = ai.NextStepSuggestion,
                            InteractionSuggested = new LeadInteraction
                            {
                                TypeOfContact = Enum.Parse<TypesOfContact>(ai.SuggestedContactType),
                                InteractionContent = ai.NextStepSuggestion
                            }
                        };
                    }
                }
            }
            catch { }

            return new Suggestion
            {
                Notes = "Continue acompanhando o lead.",
                InteractionSuggested = new LeadInteraction { TypeOfContact = TypesOfContact.Email }
            };
        }

        public Priorities DeterminePriority(int score, int interactionCount)
        {
            return score >= 85 ? Priorities.Urgent :
                   score >= 65 ? Priorities.High :
                   score >= 40 ? Priorities.Medium : Priorities.Low;
        }
    }

    public class AIResponseModel
    {
        public int Score { get; set; }
        public double Probability { get; set; }
        public string Priority { get; set; } = "Medium";
        public string NextStepSuggestion { get; set; } = "";
        public string SuggestedContactType { get; set; } = "Email";
    }
}