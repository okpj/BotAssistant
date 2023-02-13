namespace BotAssistant.Infrastructure.Yandex.Model.Speech;

public class RecognizeResponse : BaseResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }
}
