namespace BotAssistant.Application.Contract.YandexCloud.Model.Speech;

public class Chunk
{
    [JsonPropertyName("alternatives")]
    public Alternative[] Alternatives { get; set; }
}