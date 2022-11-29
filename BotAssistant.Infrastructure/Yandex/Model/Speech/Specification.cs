namespace BotAssistant.Infrastructure.Yandex.Model.Speech;

public class Specification
{
    [JsonPropertyName("languageCode")]
    public string LanguageCode => "auto";

    [JsonPropertyName("literature_text")]
    public bool LiteratureText { get; set; }

    [JsonPropertyName("rawResults")]
    public bool RawResults { get; set; }
}
