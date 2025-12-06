using BuddyTech.API.Models;

namespace BuddyTech.API.Services
{
    public interface ILeadService
    {
        Task<IEnumerable<Lead>> GetAllLeadsAsync();
        Task<Lead> GetLeadByIdAsync(Guid id);
        Task<IEnumerable<Lead>> GetLeadsBySellerIdAsync(Guid sellerId);
        Task<Lead> CreateLeadAsync(Lead newLead);
        Task<Lead> UpdateLeadAsync(Lead updatedLead);
        Task DeleteLeadAsync(Guid id);
        Task<Lead> AddInteractionAndDispatchAnalysisAsync(Guid leadId, LeadInteraction interaction);

    }
}