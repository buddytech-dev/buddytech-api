using BuddyTech.API.Enums;
using BuddyTech.API.Models;
using System.Net.Http.Json; // Usar para métodos como PostAsJsonAsync
using System.Text.Json; // Usar para serialização/desserialização

namespace BuddyTech.API.Services
{
    // Modelo para desserializar a resposta do serviço Python
    // O Python deve retornar um JSON com um campo "score" e "suggestion"
    public class AIResponseModel
    {
        public int Score { get; set; }
        public string SuggestionNotes { get; set; }
        public string SuggestedContactType { get; set; }
    }

    public class ScoringService : IScoringService
    {
        private readonly HttpClient _httpClient;

        // HttpClient é injetado pelo .NET Core (configurado no Program.cs com AddHttpClient)
        public ScoringService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // 🚨 MÉTODO DE INTEGRAÇÃO COM A IA (Python) 🚨
        public async Task<LeadScore> CalculateScoreAsync(Lead lead)
        {
            try
            {
                // 1. Prepara o payload para enviar ao serviço Python
                var payload = new
                {
                    LeadId = lead.Id,
                    Interactions = lead.Interactions,
                    CompanyRevenue = lead.Company.RevenueRange.ToString(),
                    LeadStatus = lead.Status.ToString(),
                    // ... adicione outros dados relevantes para a IA aqui
                };

                // 2. Chama o endpoint do serviço Python (Ex: http://localhost:8000/api/score)
                var response = await _httpClient.PostAsJsonAsync("api/score", payload);

                if (response.IsSuccessStatusCode)
                {
                    // 3. Lê e desserializa a resposta do Python
                    var aiData = await response.Content.ReadFromJsonAsync<AIResponseModel>();

                    if (aiData != null)
                    {
                        return new LeadScore
                        {
                            Score = aiData.Score,
                            UpdatedAt = DateOnly.FromDateTime(DateTime.Now)
                        };
                    }
                }

                // Logar o erro de resposta HTTP aqui (response.StatusCode)
            }
            catch (Exception ex)
            {
                // Logar o erro de comunicação (ex: serviço Python fora do ar)
                Console.WriteLine($"Erro ao comunicar com a AI: {ex.Message}");
            }

            // Fallback (se a AI falhar, retorna um score neutro)
            return new LeadScore { Score = 20, UpdatedAt = DateOnly.FromDateTime(DateTime.Now) };
        }

        // MÉTODO MANTIDO NO C#: Determinar Prioridade (Lógica de Domínio)
        public Priorities DeterminePriority(int score, int interactionCount)
        {
            if (score >= 80) return Priorities.Urgent;
            if (score >= 50 && interactionCount > 3) return Priorities.High;
            if (score >= 30) return Priorities.Medium;
            return Priorities.Low;
        }

        // MÉTODO DE SUGESTÃO: Pode ser feito pela IA no Python ou aqui (manteremos a simulação simplificada aqui para o MVP)
        public async Task<Suggestion> GenerateSuggestionAsync(Lead lead, int currentScore)
        {
            // Na implementação real com a AI, você usaria o SuggestionNotes do AIResponseModel
            // Para o MVP, mantemos a lógica simples de simulação:

            string notes;
            TypesOfContact nextContactType;

            if (currentScore >= 80)
            {
                notes = "O lead está altamente engajado! Sugira o fechamento ou envio do contrato final. Contate o decisor.";
                nextContactType = TypesOfContact.Meeting;
            }
            else if (currentScore >= 50)
            {
                notes = "Foco em qualificação. Tente marcar uma reunião de descoberta para entender as dores e apresentar a solução.";
                nextContactType = TypesOfContact.PhoneCall;
            }
            else
            {
                notes = "Lead frio. Sugira um e-mail de nutrição com um caso de sucesso relevante para a indústria dele.";
                nextContactType = TypesOfContact.Email;
            }

            return new Suggestion
            {
                Notes = $"AI Suggestion ({currentScore}%): {notes}",
                InteractionSuggested = new LeadInteraction
                {
                    TypeOfContact = nextContactType,
                    InteractionContent = notes
                }
            };
        }
    }
}