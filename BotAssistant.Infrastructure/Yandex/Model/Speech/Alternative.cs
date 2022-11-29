namespace BotAssistant.Infrastructure.Yandex.Model.Speech;

public class Alternative
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
