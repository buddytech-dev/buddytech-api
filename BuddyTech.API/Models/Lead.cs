using BuddyTech.API.Enums;
using Microsoft.VisualBasic;

namespace BuddyTech.API.Models
{
    public class Lead
    {
        public Guid Id { get; set; }

        public Guid SellerId { get; set; }
        public Seller Seller { get; set; }

        public Guid CompanyId { get; set; }
        public Company Company { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string LeadSource { get; set; }
        public LeadStatus Status { get; set; }
        public bool Won { get; set; }
        public DateOnly CreatedAt { get; set; }
        public DateOnly CloseDate { get; set; }

        public ICollection<LeadInteraction> Interactions { get; set; }
        
        // Serão preenchidos pela IA
        public LeadScore? CurrentScore { get; set; }
        public Guid? CurrentScoreId { get; set; }

        public ICollection<LeadScore>? ScoreHistory { get; set; }
        public double ProbabilityOfClosing { get; set; }
        public DateOnly ExpectedCloseDate { get; set; }
        public Priorities Priority { get; set; }
        public Guid? SuggestionId { get; set; }
        public Suggestion? Suggestion { get; set; }
        public Lead() { }
    }
}
