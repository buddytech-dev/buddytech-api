using BuddyTech.API.Enums;

namespace BuddyTech.API.DTOs.Leads
{
    public class LeadUpdateRequestDto
    {
        public Guid Id { get; set; }
        public Guid SellerId { get; set; }
        public Guid? CompanyId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? LeadSource { get; set; }
        public LeadStatus? Status { get; set; }
    }
}
