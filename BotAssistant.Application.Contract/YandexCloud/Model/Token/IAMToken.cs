namespace BotAssistant.Application.Contract.YandexCloud.Model.Token;

/// <summary>
/// IAM Токен
/// </summary>
public struct IAMToken
{
    [JsonPropertyName("iamToken")]
    public string Token { get; set; }

    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; set; }
}
