using BuddyTech.API.Infra;
using BuddyTech.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BuddyTech.API.Services
{
    public class LeadService : ILeadService
    {
        private readonly ApplicationDbContext _context;
        private readonly IScoringService _scoringService; // Dependência da IA

        public LeadService(ApplicationDbContext context, IScoringService scoringService)
        {
            _context = context;
            _scoringService = scoringService;
        }

        public async Task<Lead> GetLeadByIdAsync(Guid id)
        {
            // Inclui as interações, pontuação e sugestões mais recentes
            return await _context.Leads
                .Include(l => l.Interactions)
                .Include(l => l.CurrentScore)
                .Include(l => l.Suggestion)
                .Include(l => l.Company)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        // ... (Implementação de outros métodos CRUD omitidos para brevidade) ...

        // Método-chave para o Desafio: Atualiza Interações e dispara o Scoring
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