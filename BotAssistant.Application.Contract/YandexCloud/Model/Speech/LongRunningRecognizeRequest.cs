namespace BotAssistant.Application.Contract.YandexCloud.Model.Speech;

public class LongRunningRecognizeRequest
{
    [JsonPropertyName("config")]
    public Config Config { get; set; }

    [JsonPropertyName("audio")]
    public Audio Audio { get; set; }
}


