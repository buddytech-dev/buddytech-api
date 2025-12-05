using BuddyTech.API.Enums;
using BuddyTech.API.Models;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Linq;

namespace BuddyTech.API.Services
{
    // 🚨 MÉTODO DE INTEGRAÇÃO COM A IA (SCORE) 🚨
    public class ScoringService : IScoringService
    {
        private readonly HttpClient _httpClient;

        public ScoringService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método auxiliar para criar o DTO e evitar repetição de código
        private LeadContextInput BuildAiPayload(Lead lead)
        {
            return new LeadContextInput
            {
                // A propriedade [JsonPropertyName("industry")] no DTO garante o nome correto no JSON
                Industry = lead.Company?.Industry ?? "Genérico",

                // Converte o Enum para int, já que o Python espera um inteiro em "revenue_range"
                RevenueRange = (int)(lead.Company?.RevenueRange ?? 0),

                // Mapeia para "current_stage"
                CurrentStage = lead.Status.ToString(),

                // Mapeia a lista de interações
                Interactions = lead.Interactions?
                    .OrderByDescending(i => i.InteractionDate)
                    .Take(10) // Padronizei em 10 para dar bom contexto à IA
                    .Select(i => new InteractionInput
                    {
                        Date = i.InteractionDate.ToString("yyyy-MM-dd"),
                        Type = i.TypeOfContact.ToString(),
                        Content = i.InteractionContent
                    })
                    .ToList() ?? new List<InteractionInput>()
            };
        }

        public async Task<LeadScore> CalculateScoreAsync(Lead lead)
        {
            try
            {
                // 1. Usa o DTO ao invés de objeto anônimo
                var payload = BuildAiPayload(lead);

                // 2. Chama o endpoint enviando o DTO tipado
                var response = await _httpClient.PostAsJsonAsync("analyze", payload);

                if (response.IsSuccessStatusCode)
                {
                    var aiData = await response.Content.ReadFromJsonAsync<AiServiceResponse>();

                    if (aiData != null)
                    {
                        return new LeadScore
                        {
                            Score = aiData.Score,
                            UpdatedAt = DateOnly.FromDateTime(DateTime.Now)
                        };
                    }
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"AI Service Error ({response.StatusCode}): {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao comunicar com a AI: {ex.Message}");
            }

            // Fallback
            return new LeadScore { Score = 20, UpdatedAt = DateOnly.FromDateTime(DateTime.Now) };
        }

        public async Task<Suggestion> GenerateSuggestionAsync(Lead lead, int currentScore)
        {
            try
            {
                // 1. Usa o mesmo DTO. 
                // Nota: O Python recalculava o score internamente, então enviamos o contexto completo novamente.
                var payload = BuildAiPayload(lead);

                var response = await _httpClient.PostAsJsonAsync("analyze", payload);

                if (response.IsSuccessStatusCode)
                {
                    var aiData = await response.Content.ReadFromJsonAsync<AiServiceResponse>();

                    if (aiData != null)
                    {
                        var contactType = (TypesOfContact)aiData.NextStepType;

                        return new Suggestion
                        {
                            Id = Guid.NewGuid(),
                            Notes = $"[IA Score: {aiData.Score}]: {aiData.Notes}",
                            InteractionSuggested = new LeadInteraction
                            {
                                TypeOfContact = contactType,
                                InteractionContent = aiData.Notes,
                                InteractionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(2))
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[IA ERROR] Falha ao conectar na API Python: {ex.Message}");
            }

            // FALLBACK (Mantido igual)
            string fallbackNotes;
            TypesOfContact fallbackType;

            if (currentScore >= 80)
            {
                fallbackNotes = "O lead está altamente engajado! Sugira o fechamento.";
                fallbackType = TypesOfContact.Meeting;
            }
            else if (currentScore >= 50)
            {
                fallbackNotes = "Foco em qualificação. Tente marcar uma reunião.";
                fallbackType = TypesOfContact.PhoneCall;
            }
            else
            {
                fallbackNotes = "Lead frio. Sugira um e-mail de nutrição.";
                fallbackType = TypesOfContact.Email;
            }

            return new Suggestion
            {
                Id = Guid.NewGuid(),
                Notes = $"[Simulação]: {fallbackNotes}",
                InteractionSuggested = new LeadInteraction
                {
                    TypeOfContact = fallbackType,
                    InteractionContent = fallbackNotes,
                    InteractionDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3))
                }
            };
        }

        public Priorities DeterminePriority(int score, int interactionCount)
        {

            if (score >= 80) return Priorities.Urgent;
            if (score >= 50 && interactionCount > 3) return Priorities.High;
            if (score >= 30) return Priorities.Medium;

            return Priorities.Low;
        }

        // CLASSE AUXILIAR DO JSON DE RESPOSTA (Mantive aqui, mas poderia ir para DTOs também)
        public class AiServiceResponse
        {
            [JsonPropertyName("score")]
            public int Score { get; set; }

            [JsonPropertyName("notes")]
            public string Notes { get; set; }

            [JsonPropertyName("next_step_type")]
            public int NextStepType { get; set; }
        }
    }
}