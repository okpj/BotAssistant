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

    public static IAMToken? FromStingJson(string json)
    {
        try
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<IAMToken>(json);
            return result;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex.Message);
            return null;
        }
    }
}
