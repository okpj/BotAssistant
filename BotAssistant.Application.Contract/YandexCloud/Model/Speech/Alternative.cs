namespace BotAssistant.Application.Contract.YandexCloud.Model.Speech;

public class Alternative
{
    [JsonPropertyName("text")]
    public string Text { get; set; }
}
