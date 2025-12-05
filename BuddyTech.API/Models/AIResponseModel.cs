namespace BuddyTech.API.Models
{
    // Modelo para desserializar a resposta do serviço Python
    // O Python deve retornar um JSON com um campo "score" e "suggestion"
    public class AIResponseModel
    {
        public int Score { get; set; }
        public string SuggestionNotes { get; set; }
        public string SuggestedContactType { get; set; }
    }
}
