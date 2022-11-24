namespace BotAssistant.Infrastructure.Yandex.Model.Speech;

public class Chunk
{
    [JsonPropertyName("alternatives")]
    public Alternative[] Alternatives { get; set; }
}
