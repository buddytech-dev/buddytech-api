using BuddyTech.API.Enums;
using BuddyTech.API.Models;

namespace BuddyTech.API.Services
{
    public interface IScoringService
    {
        Task<LeadScore> CalculateScoreAsync(Lead lead);
        Task<Suggestion> GenerateSuggestionAsync(Lead lead, int currentScore);
        Priorities DeterminePriority(int score, int interactionCount);
    }
}