namespace BotAssistant.Infrastructure.Yandex.Model;

public class BaseOperation : BaseResponse
{
    [JsonPropertyName("done")]
    public bool Done { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}
