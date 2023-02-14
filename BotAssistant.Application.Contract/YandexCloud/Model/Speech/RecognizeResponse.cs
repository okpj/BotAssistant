namespace BotAssistant.Application.Contract.YandexCloud.Model.Speech;

public class RecognizeResponse : BaseResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }
}
