namespace BotAssistant.Infrastructure.Yandex.Model.Speech;

public class LongRunningRecognizeResponse
{
    [JsonPropertyName("chunks")]
    public Chunk[]  Chunks { get; set; }
}
