using BuddyTech.API.DTOs;
using BuddyTech.API.Models;
using BuddyTech.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace BuddyTech.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeadController : ControllerBase
    {
        private readonly ILeadService _leadService;
        private readonly IMissionService _missionService; // Será usado para a gamificação

        public LeadController(ILeadService leadService, IMissionService missionService)
        {
            _leadService = leadService;
            _missionService = missionService;
        }

        // POST /api/Lead
        /// <summary>
        /// Cria um novo Lead e a Companhia associada.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateLead([FromBody] LeadCreateRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // 1. Mapear DTO para Model (Usaríamos AutoMapper aqui, mas faremos manualmente)
            var newCompany = new Company
            {
                Name = dto.CompanyName,
                Email = dto.CompanyEmail,
                Industry = dto.Industry,
                RevenueRange = dto.RevenueRange,
                Id = Guid.NewGuid() // Garante ID
            };

            var newLead = new Lead
            {
                Title = dto.Title,
                Description = dto.Description,
                LeadSource = dto.LeadSource,
                SellerId = dto.SellerId,
                Company = newCompany,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                Status = Enums.LeadStatus.New,
                // A prioridade inicial e o score serão definidos pelo Service/IA
            };

            var createdLead = await _leadService.CreateLeadAsync(newLead);

            // 2. Aciona o Serviço de Missões após a criação do Lead
            await _missionService.CheckMissionCompletionAsync(createdLead.SellerId, Enums.MissionTitles.FirstLeadCreated);

            return CreatedAtAction(nameof(GetLead), new { id = createdLead.Id }, createdLead);
        }

        // GET /api/Lead/seller/{sellerId}
        /// <summary>
        /// Obtém todos os Leads de um vendedor, ordenados por Prioridade e Score (Painel de Priorização).
        /// </summary>
        [HttpGet("seller/{sellerId}")]
        public async Task<IActionResult> GetPrioritizedLeads(Guid sellerId)
        {
            var leads = await _leadService.GetLeadsBySellerIdAsync(sellerId);

            // 1. Ordenação para Priorização (O principal do desafio!)
            // Ordena leads com base na Prioridade (Urgent > High > Medium > Low) e depois pelo Score.
            var prioritizedLeads = leads
                .OrderByDescending(l => (int)l.Priority) // Cast para int para ordenar pelo Enum (3, 2, 1, 0)
                .ThenByDescending(l => l.CurrentScore?.Score ?? 0)
                .Select(l => new LeadResponseDto
                {
                    LeadId = l.Id,
                    Title = l.Title,
                    Status = l.Status.ToString(),
                    CompanyName = l.Company.Name,
                    // SellerName precisaria ser carregado via Include ou outro Service
                    CurrentScore = l.CurrentScore?.Score ?? 0,
                    ProbabilityOfClosing = l.ProbabilityOfClosing,
                    Priority = l.Priority.ToString(),
                    NextStepSuggestion = l.Suggestion?.Notes ?? "Nenhuma sugestão ainda.",
                    InteractionsCount = l.Interactions?.Count ?? 0,
                    ExpectedCloseDate = l.ExpectedCloseDate
                }).ToList();

            return Ok(prioritizedLeads);
        }

        // GET /api/Lead/{id}
        /// <summary>
        /// Obtém um Lead detalhado.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetLead(Guid id)
        {
            var lead = await _leadService.GetLeadByIdAsync(id);
            if (lead == null)
            {
                return NotFound($"Lead com ID {id} não encontrado.");
            }

            // Mapeia para DTO (Retorno do seu Controller)
            return Ok(new LeadResponseDto
            {
                LeadId = lead.Id,
                Title = lead.Title,
                Status = lead.Status.ToString(),
                CompanyName = lead.Company.Name,
                CurrentScore = lead.CurrentScore?.Score ?? 0,
                ProbabilityOfClosing = lead.ProbabilityOfClosing,
                Priority = lead.Priority.ToString(),
                NextStepSuggestion = lead.Suggestion?.Notes ?? "Nenhuma sugestão ainda.",
                SuggestedContactType = lead.Suggestion?.InteractionSuggested?.TypeOfContact.ToString() ?? "N/A",
                InteractionsCount = lead.Interactions?.Count ?? 0,
                ExpectedCloseDate = lead.ExpectedCloseDate
            });
        }

        // POST /api/Lead/{id}/interact
        /// <summary>
        /// Registra uma nova interação com o Lead e recalcula o Lead Score e Sugestões da IA.
        /// </summary>
        [HttpPost("{id}/interact")]
        public async Task<IActionResult> AddInteraction(Guid id, [FromBody] LeadInteractionRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Mapeia DTO para Model (LeadInteraction)
                var interaction = new LeadInteraction
                {
                    InteractionContent = dto.InteractionContent,
                    TypeOfContact = dto.TypeOfContact,
                    InteractionDate = DateOnly.FromDateTime(DateTime.Now)
                };

                // Chama a lógica de negócio que inclui o scoring da IA
                var updatedLead = await _leadService.AddInteractionAndRecalculateScoreAsync(id, interaction);

                // Mapeia o Lead atualizado para o DTO de Resposta
                return Ok(new LeadResponseDto
                {
                    LeadId = updatedLead.Id,
                    Title = updatedLead.Title,
                    Status = updatedLead.Status.ToString(),
                    CompanyName = updatedLead.Company.Name,
                    CurrentScore = updatedLead.CurrentScore.Score,
                    ProbabilityOfClosing = updatedLead.ProbabilityOfClosing,
                    Priority = updatedLead.Priority.ToString(),
                    NextStepSuggestion = updatedLead.Suggestion.Notes,
                    SuggestedContactType = updatedLead.Suggestion.InteractionSuggested.TypeOfContact.ToString(),
                    InteractionsCount = updatedLead.Interactions.Count,
                    ExpectedCloseDate = updatedLead.ExpectedCloseDate
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Logar o erro (ex: falha de comunicação com a IA)
                return StatusCode(500, $"Erro interno ao processar a interação: {ex.Message}");
            }
        }
    }
}