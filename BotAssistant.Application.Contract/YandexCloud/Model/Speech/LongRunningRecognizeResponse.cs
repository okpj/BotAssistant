namespace BotAssistant.Application.Contract.YandexCloud.Model.Speech;

public class LongRunningRecognizeResponse
{
    [JsonPropertyName("chunks")]
    public Chunk[] Chunks { get; set; }
}