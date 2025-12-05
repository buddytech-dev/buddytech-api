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

        public async Task<IEnumerable<Lead>> GetAllLeadsAsync()
        {
            return await _context.Leads
                .Include(l => l.CurrentScore)
                .Include(l => l.Suggestion)
                .Include(l => l.Company)
                .ToListAsync();
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

        public async Task<Lead> AddInteractionAndDispatchAnalysisAsync(Guid leadId, LeadInteraction interaction)
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

            // 1) Persist the new interaction first to ensure FK constraints
            interaction.LeadId = leadId;
            _context.LeadInteractions.Add(interaction);
            await _context.SaveChangesAsync();

            // Refresh local collection
            lead.Interactions ??= new List<LeadInteraction>();
            lead.Interactions.Add(interaction);

            // 2) Call AI services
            var newScore = await _scoringService.CalculateScoreAsync(lead);
            var newSuggestion = await _scoringService.GenerateSuggestionAsync(lead, newScore.Score);

            // 3) Prepare and persist the new score
            newScore.LeadId = leadId;
            newScore.Lead = lead;
            _context.LeadScores.Add(newScore);

            // 4) If AI created a suggested interaction, persist it first and set its LeadId
            if (newSuggestion.InteractionSuggested != null)
            {
                newSuggestion.InteractionSuggested.LeadId = leadId;
                _context.LeadInteractions.Add(newSuggestion.InteractionSuggested);
                await _context.SaveChangesAsync();

                // Ensure the FK in Suggestion points to the persisted interaction
                newSuggestion.InteractionSuggestedId = newSuggestion.InteractionSuggested.Id;
            }

            // 5) Link suggestion to lead and persist suggestion
            newSuggestion.LeadId = leadId;
            newSuggestion.Lead = lead;
            _context.Suggestions.Add(newSuggestion);

            // 6) Update lead aggregates and persist everything
            lead.CurrentScore = newScore;
            lead.ScoreHistory ??= new List<LeadScore>();
            lead.ScoreHistory.Add(newScore);
            lead.Suggestion = newSuggestion;

            lead.ProbabilityOfClosing = newScore.Score / 100.0;
            lead.Priority = _scoringService.DeterminePriority(newScore.Score, lead.Interactions.Count);

            // set FK ids on lead
            lead.CurrentScoreId = newScore.Id;
            lead.SuggestionId = newSuggestion.Id;

            await _context.SaveChangesAsync();
            return lead;
        }
    }
}