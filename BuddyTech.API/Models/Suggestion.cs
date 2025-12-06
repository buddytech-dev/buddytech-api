namespace BuddyTech.API.Models
{
    public class Suggestion
    {
        public Guid Id { get; set; }

        // Explicit FK to the suggested interaction (matches migration column)
        public Guid InteractionSuggestedId { get; set; }
        public LeadInteraction InteractionSuggested { get; set; }

        // Make Lead relationship optional to avoid required FK issues when dissociating
        public Guid? LeadId { get; set; }
        public Lead? Lead { get; set; }

        public string Notes { get; set; }
        public Suggestion() { }
    }
}
