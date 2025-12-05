using BuddyTech.API.Enums;
using BuddyTech.API.Models;

namespace BuddyTech.API.Services
{
    public interface IScoringService
    {
        Task<LeadScore> CalculateScoreAsync(Lead lead);
        Priorities DeterminePriority(int score, int interactionCount);
        Task<Suggestion> GenerateSuggestionAsync(Lead lead, int currentScore);
    }
}