namespace BuddyTech.API.Models
{
    public class Suggestion
    {
        public Guid Id { get; set; }
        public string Notes { get; set; }
        public LeadInteraction InteractionSuggested { get; set; }
        public Suggestion() { }
    }
}
