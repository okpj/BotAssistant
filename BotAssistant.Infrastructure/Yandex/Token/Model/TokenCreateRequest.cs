namespace BotAssistant.Infrastructure.Yandex.Token.Model;

/// <summary>
/// запрос создания токена
/// </summary>
public struct TokenCreateRequest
{
    /// <summary>
    /// Json Web Token
    /// </summary>
    [JsonPropertyName("jwt")]
    public string JWT { get; set; }
}
