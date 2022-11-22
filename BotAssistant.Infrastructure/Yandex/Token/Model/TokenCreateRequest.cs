namespace BotAssistant.Infrastructure.Yandex.Token.Model;

public struct TokenCreateRequest
{
    [JsonPropertyName("jwt")]
    public string JWT { get; set; }
}
