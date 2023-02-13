namespace BotAssistant.Infrastructure.Yandex.Token.Model;

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
