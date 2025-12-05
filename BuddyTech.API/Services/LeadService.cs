using BuddyTech.API.Infra;
using BuddyTech.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BuddyTech.API.Services
{
    public class LeadService : ILeadService
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoringService _scoringService;

        public LeadService(ApplicationDbContext context, IScoringService scoringService)
        {
            _context = context;
            _scoringService = scoringService;
        }

        public async Task<IEnumerable<Lead>> GetLeadsBySellerIdAsync(Guid sellerId)
        {
            return await _context.Leads
                .Include(l => l.CurrentScore)
                .Include(l => l.Suggestion)
                .Include(l => l.Company)
                .Where(l => l.SellerId == sellerId)
                .ToListAsync();
        }

        public async Task<Lead> GetLeadByIdAsync(Guid id)
        {
            return await _context.Leads
                .Include(l => l.Interactions)
                .Include(l => l.CurrentScore)
                .Include(l => l.Suggestion)
                .Include(l => l.Company)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<Lead> CreateLeadAsync(Lead newLead)
        {
            if (newLead.CompanyId != Guid.Empty && newLead.Company != null)
            {
                newLead.Company = null;
            }
            else if (newLead.CompanyId == Guid.Empty && (newLead.Company == null || string.IsNullOrEmpty(newLead.Company.CNPJ)))
            {
                throw new InvalidOperationException("É necessário informar um CompanyId existente ou os dados completos de uma nova Company.");
            }
            newLead.CurrentScore = null;
            newLead.Suggestion = null;
            newLead.ScoreHistory = null;

            _context.Leads.Add(newLead);
            await _context.SaveChangesAsync(); 

            var initialScore = await _scoringService.CalculateScoreAsync(newLead);
            var initialSuggestion = await _scoringService.GenerateSuggestionAsync(newLead, initialScore.Score);

            initialScore.LeadId = newLead.Id;
            initialScore.Lead = newLead;

            initialSuggestion.LeadId = newLead.Id;
            initialSuggestion.Lead = newLead;

            if (initialSuggestion.InteractionSuggested != null)
            {
                initialSuggestion.InteractionSuggested.LeadId = newLead.Id;
            }

            _context.LeadScores.Add(initialScore);
            _context.Suggestions.Add(initialSuggestion);
            await _context.SaveChangesAsync();

            newLead.ProbabilityOfClosing = initialScore.Score / 100.0;
            newLead.Priority = _scoringService.DeterminePriority(initialScore.Score, 0);

            newLead.CurrentScoreId = initialScore.Id;
            newLead.SuggestionId = initialSuggestion.Id;

            newLead.CurrentScore = initialScore;
            newLead.Suggestion = initialSuggestion;
            newLead.ScoreHistory = new List<LeadScore> { initialScore };

            await _context.SaveChangesAsync();

            return newLead;
        }
        public async Task<Lead> UpdateLeadAsync(Lead updatedLead)
        {
            var existingLead = await _context.Leads.FindAsync(updatedLead.Id);

            if (existingLead == null)
            {
                throw new KeyNotFoundException($"Lead com ID {updatedLead.Id} não encontrado para atualização.");
            }

            _context.Entry(existingLead).CurrentValues.SetValues(updatedLead);

            await _context.SaveChangesAsync();
            return existingLead;
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var leadToDelete = await _context.Leads.FindAsync(id);
            if (leadToDelete != null)
            {
                _context.Leads.Remove(leadToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Lead> AddInteractionAndRecalculateScoreAsync(Guid leadId, LeadInteraction interaction)
        {
            var lead = await _context.Leads
                .Include(l => l.Interactions)
                .Include(l => l.CurrentScore)
                .Include(l => l.Suggestion)
                .FirstOrDefaultAsync(l => l.Id == leadId);

            if (lead == null)
            {
                throw new KeyNotFoundException($"Lead com ID {leadId} não encontrado.");
            }

            interaction.LeadId = leadId;
            lead.Interactions.Add(interaction);
            _context.LeadInteractions.Add(interaction);

            var newScore = await _scoringService.CalculateScoreAsync(lead);
            var newSuggestion = await _scoringService.GenerateSuggestionAsync(lead, newScore.Score);

            lead.CurrentScore = newScore;

            if (lead.ScoreHistory == null)
            {
                lead.ScoreHistory = new List<LeadScore>();
            }
            lead.ScoreHistory.Add(newScore);
            _context.LeadScores.Add(newScore);

            lead.Suggestion = newSuggestion;
            _context.Suggestions.Add(newSuggestion);

            lead.ProbabilityOfClosing = newScore.Score / 100.0;
            lead.Priority = _scoringService.DeterminePriority(newScore.Score, lead.Interactions.Count);

            await _context.SaveChangesAsync();
            return lead;
        }
    }
}