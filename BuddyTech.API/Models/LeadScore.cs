using System.ComponentModel.DataAnnotations;

namespace BuddyTech.API.Models
{
    public class LeadScore
    {
        public Guid Id { get; set; }

        public Guid LeadId { get; set; }
        public Lead Lead { get; set; }

        [Range(0, 100)]
        public int Score { get; set; }
        public DateOnly UpdatedAt { get; set; }
        public LeadScore() { }
    }
}
