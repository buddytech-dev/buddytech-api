using System.ComponentModel.DataAnnotations;
using BuddyTech.API.Enums;

namespace BuddyTech.API.DTOs.Leads
{
    public class LeadCreateRequestDto
    {
        [Required]
        public Guid SellerId { get; set; }

        public Guid? CompanyId { get; set; }

        public string? CompanyName { get; set; }
        public string? CompanyCNPJ { get; set; }
        public string? CompanyEmail { get; set; }
        public string? CompanyPhone { get; set; }
        public string? CompanyLocation { get; set; }
        public string? CompanyLogo { get; set; }
        public string? Industry { get; set; }
        public RevenueRanges? RevenueRange { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string LeadSource { get; set; }
    }
}