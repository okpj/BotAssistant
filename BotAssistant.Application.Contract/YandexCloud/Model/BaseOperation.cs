namespace BotAssistant.Application.Contract.YandexCloud.Model;

/// <summary>
/// Базовая операция
/// </summary>
public class BaseOperation : BaseResponse
{
    [JsonPropertyName("done")]
    public bool Done { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}