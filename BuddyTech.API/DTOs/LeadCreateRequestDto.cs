using System.ComponentModel.DataAnnotations;
using BuddyTech.API.Enums;

namespace BuddyTech.API.DTOs
{
    public class LeadCreateRequestDto
    {
        [Required]
        public Guid SellerId { get; set; }

        // Dados da Companhia (opcionalmente, você pode criar um CompanyCreateDto)
        [Required]
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string Industry { get; set; }
        public RevenueRanges RevenueRange { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string LeadSource { get; set; }
    }
}