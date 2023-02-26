namespace BotAssistant.Application.Contract.YandexCloud.Model.Token;

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