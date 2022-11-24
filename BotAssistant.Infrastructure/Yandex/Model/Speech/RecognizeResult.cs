namespace BotAssistant.Infrastructure.Yandex.Model.Speech;

public class RecognizeResult : BaseResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }
}
