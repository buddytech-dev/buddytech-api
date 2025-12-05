using System.Text.Json.Serialization; // Importante

public class LeadContextInput
{
    [JsonPropertyName("industry")]
    public string Industry { get; set; }

    // O Python espera snake_case, então forçamos aqui:
    [JsonPropertyName("revenue_range")]
    public int RevenueRange { get; set; }

    [JsonPropertyName("current_stage")]
    public string CurrentStage { get; set; }

    [JsonPropertyName("interactions")]
    public List<InteractionInput> Interactions { get; set; }
}

public class InteractionInput
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("content")]
    public string Content { get; set; }
}