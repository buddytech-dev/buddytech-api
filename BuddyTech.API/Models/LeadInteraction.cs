using BuddyTech.API.Enums;

namespace BuddyTech.API.Models
{
    public class LeadInteraction
    {
        public Guid Id { get; set; }

        public Guid LeadId { get; set; }
        public Lead Lead { get; set; }

        public TypesOfContact TypeOfContact { get; set; }
        public string InteractionContent { get; set; }
        public DateOnly InteractionDate { get; set; }
        public LeadInteraction() { }
    }
}
