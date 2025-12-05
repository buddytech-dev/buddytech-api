using BuddyTech.API.DTOs;
using BuddyTech.API.DTOs.Leads;
using BuddyTech.API.Models;
using BuddyTech.API.Services;
using Microsoft.AspNetCore.Mvc;
using BuddyTech.API.Enums;

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

            // Mapeamento Manual Inteligente
            var newLead = new Lead
            {
                Title = dto.Title,
                Description = dto.Description,
                LeadSource = dto.LeadSource,
                SellerId = dto.SellerId,
                CreatedAt = DateOnly.FromDateTime(DateTime.Now),
                Status = Enums.LeadStatus.New
            };

            // Lógica de Decisão: Empresa Existente vs Nova
            if (dto.CompanyId.HasValue && dto.CompanyId.Value != Guid.Empty)
            {
                // CENÁRIO 1: Empresa Existente
                newLead.CompanyId = dto.CompanyId.Value;
                newLead.Company = null; // Garante que não vai tentar criar nada
            }
            else
            {
                // CENÁRIO 2: Criar Nova Empresa
                // Validação manual simples
                if (string.IsNullOrEmpty(dto.CompanyName) || string.IsNullOrEmpty(dto.CompanyCNPJ))
                {
                    return BadRequest("Para criar uma nova empresa, 'CompanyName' e 'CompanyCNPJ' são obrigatórios.");
                }

                newLead.Company = new Company
                {
                    Id = Guid.NewGuid(),
                    Name = dto.CompanyName,
                    CNPJ = dto.CompanyCNPJ,
                    Email = dto.CompanyEmail,
                    Phone = dto.CompanyPhone ?? "Não Informado",
                    Location = dto.CompanyLocation ?? "Localização Desconhecida",
                    Logo = dto.CompanyLogo ?? "https://via.placeholder.com/150",
                    Industry = dto.Industry ?? "Outros",
                    RevenueRange = dto.RevenueRange ?? Enums.RevenueRanges.UpTo500K
                };
            }

            try
            {
                var createdLead = await _leadService.CreateLeadAsync(newLead);

                // Aciona gamificação
                await _missionService.CheckMissionCompletionAsync(createdLead.SellerId, Enums.MissionTitles.FirstLeadCreated);

                return CreatedAtAction(nameof(GetLead), new { id = createdLead.Id }, new { id = createdLead.Id, message = "Lead criado com sucesso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message); // Retorna erro amigável se o Service reclamar
            }
            catch (Exception ex)
            {
                // Logar o erro real no console para debug
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Erro interno ao criar Lead.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLeads()
        {
            var leads = await _leadService.GetAllLeadsAsync();
            var response = leads.Select(l => new LeadResponseDto
            {
                LeadId = l.Id,
                Title = l.Title,
                Status = l.Status.ToString(),
                CompanyName = l.Company?.Name ?? "Empresa não informada",
                CompanyEmail = l.Company?.Email ?? string.Empty,
                CompanyCNPJ = l.Company?.CNPJ ?? string.Empty,
                CompanyRevenueRange = l.Company != null ? ((RevenueRanges)l.Company.RevenueRange).ToString() : string.Empty,
                CompanyPhone = l.Company?.Phone ?? string.Empty,
                Industry = l.Company?.Industry ?? string.Empty,
                SellerName = l.Seller?.Name ?? string.Empty,
                CurrentScore = l.CurrentScore?.Score ?? 0,
                ProbabilityOfClosing = l.ProbabilityOfClosing,
                Priority = l.Priority.ToString(),
                NextStepSuggestion = l.Suggestion?.Notes ?? "Nenhuma sugestão ainda.",
                SuggestedContactType = l.Suggestion?.InteractionSuggested?.TypeOfContact.ToString() ?? "N/A",
                InteractionsCount = l.Interactions?.Count ?? 0,
                ExpectedCloseDate = l.ExpectedCloseDate
            }).ToList();
            return Ok(response);
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
                CompanyEmail = lead.Company.Email,
                CompanyCNPJ = lead.Company.CNPJ,
                CompanyPhone = lead.Company.Phone,
                CompanyRevenueRange = lead.Company.RevenueRange.ToString(),
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
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var interaction = new LeadInteraction
                {
                    InteractionContent = dto.InteractionContent,
                    TypeOfContact = dto.TypeOfContact,
                    InteractionDate = DateOnly.FromDateTime(DateTime.Now)
                };

                // O Lead volta RÁPIDO. A IA ainda está rodando em outra thread.
                var updatedLead = await _leadService.AddInteractionAndDispatchAnalysisAsync(id, interaction);

                // Mapeia com SEGURANÇA (Null Checks)
                return Ok(new LeadResponseDto
                {
                    LeadId = updatedLead.Id,
                    Title = updatedLead.Title,
                    Status = updatedLead.Status.ToString(),

                    // Null check: Se Company for null, retorna string vazia
                    CompanyName = updatedLead.Company?.Name ?? "Empresa não informada",

                    // 🚨 CRÍTICO: Se CurrentScore for null (IA rodando), retorna 0
                    CurrentScore = updatedLead.CurrentScore?.Score ?? 0,

                    ProbabilityOfClosing = updatedLead.ProbabilityOfClosing,
                    Priority = updatedLead.Priority.ToString(),

                    // 🚨 CRÍTICO: Se Suggestion for null, avisa o front
                    NextStepSuggestion = updatedLead.Suggestion?.Notes ?? "A IA está analisando...",

                    // 🚨 CRÍTICO: Navegação segura profunda
                    SuggestedContactType = updatedLead.Suggestion?.InteractionSuggested?.TypeOfContact.ToString() ?? "Aguardando...",

                    InteractionsCount = updatedLead.Interactions?.Count ?? 0,
                    ExpectedCloseDate = updatedLead.ExpectedCloseDate
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLead(Guid id, [FromBody] LeadUpdateRequestDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var existingLead = await _leadService.GetLeadByIdAsync(id);
            if (existingLead == null)
            {
                return NotFound($"Lead com ID {id} não encontrado para atualização.");
            }
            // Atualiza os campos permitidos
            existingLead.Title = dto.Title ?? existingLead.Title;
            existingLead.Description = dto.Description ?? existingLead.Description;
            existingLead.Status = dto.Status.HasValue ? dto.Status.Value : existingLead.Status;
            try
            {
                var updatedLead = await _leadService.UpdateLeadAsync(existingLead);
                return Ok(new LeadResponseDto
                {
                    LeadId = updatedLead.Id,
                    Title = updatedLead.Title,
                    Status = updatedLead.Status.ToString(),
                    CompanyName = updatedLead.Company.Name,
                    CurrentScore = updatedLead.CurrentScore?.Score ?? 0,
                    ProbabilityOfClosing = updatedLead.ProbabilityOfClosing,
                    Priority = updatedLead.Priority.ToString(),
                    NextStepSuggestion = updatedLead.Suggestion?.Notes ?? "Nenhuma sugestão ainda.",
                    InteractionsCount = updatedLead.Interactions?.Count ?? 0,
                    ExpectedCloseDate = updatedLead.ExpectedCloseDate
                });
            }
            catch (Exception ex)
            {
                // Logar o erro real no console para debug
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Erro interno ao atualizar Lead.");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLead(Guid id)
        {
            var existingLead = await _leadService.GetLeadByIdAsync(id);
            if (existingLead == null)
            {
                return NotFound($"Lead com ID {id} não encontrado para exclusão.");
            }
            try
            {
                await _leadService.DeleteLeadAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return StatusCode(500, "Erro interno ao excluir Lead.");
            }
        }
    }
}