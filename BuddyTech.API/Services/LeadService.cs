using BuddyTech.API.Infra;
using BuddyTech.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BuddyTech.API.Services
{
    public class LeadService : ILeadService  // ADICIONE ISSO AQUI!
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoringService _scoringService;

        public LeadService(ApplicationDbContext context, IScoringService scoringService)
        {
            _context = context;
            _scoringService = scoringService;
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

        public async Task<IEnumerable<Lead>> GetLeadsBySellerIdAsync(Guid sellerId)
        {
            return await _context.Leads
                .Include(l => l.Interactions)
                .Include(l => l.CurrentScore)
                .Include(l => l.Suggestion)
                .Include(l => l.Company)
                .Where(l => l.SellerId == sellerId)
                .ToListAsync();
        }

        public async Task<Lead> CreateLeadAsync(Lead newLead)
        {
            _context.Leads.Add(newLead);
            await _context.SaveChangesAsync();
            return newLead;
        }

        public async Task<Lead> UpdateLeadAsync(Lead updatedLead)
        {
            _context.Leads.Update(updatedLead);
            await _context.SaveChangesAsync();
            return updatedLead;
        }

        public async Task DeleteLeadAsync(Guid id)
        {
            var lead = await GetLeadByIdAsync(id);
            if (lead != null)
            {
                _context.Leads.Remove(lead);
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
            // 1. Adiciona a nova interação
            interaction.LeadId = leadId;
            lead.Interactions.Add(interaction);
            _context.LeadInteractions.Add(interaction);
            // 2. Dispara o serviço de Scoring (A LÓGICA DE AI ESTÁ AQUI)
            var newScore = await _scoringService.CalculateScoreAsync(lead);
            var newSuggestion = await _scoringService.GenerateSuggestionAsync(lead, newScore.Score);
            // 3. Atualiza o Lead
            lead.CurrentScore = newScore;
            lead.ScoreHistory.Add(newScore);
            lead.Suggestion = newSuggestion;
            // A prioridade e probabilidade também devem ser atualizadas pelo ScoringService
            lead.ProbabilityOfClosing = newScore.Score / 100.0;
            lead.Priority = _scoringService.DeterminePriority(newScore.Score, lead.Interactions.Count);
            await _context.SaveChangesAsync();
            return lead;
        }
    }
}