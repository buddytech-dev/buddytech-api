using BuddyTech.API.Enums;

namespace BuddyTech.API.DTOs.Leads
{
    public class LeadResponseDto
    {
        public Guid LeadId { get; set; }
        public string Title { get; set; }
        public string Status { get; set; }
        public string CompanyName { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyCNPJ { get; set; }
        public string CompanyRevenueRange { get; set; }
        public string CompanyPhone { get; set; }
        public string Industry { get; set; }
        public string SellerName { get; set; }
        public int CurrentScore { get; set; }
        public double ProbabilityOfClosing { get; set; }
        public string Priority { get; set; }
        public string NextStepSuggestion { get; set; }
        public string SuggestedContactType { get; set; }
        public int InteractionsCount { get; set; }
        public DateOnly ExpectedCloseDate { get; set; }
    }
}