namespace BotAssistant.Application.Contract.YandexCloud.Model.Speech;

public class Specification
{
    public Specification()
    { }

    public Specification(bool literatureText, bool rawResults)
    {
        LiteratureText = literatureText;
        RawResults = rawResults;
    }

    [JsonPropertyName("languageCode")]
    public static string LanguageCode => "auto";

    [JsonPropertyName("literature_text")]
    public bool LiteratureText { get; set; }

    [JsonPropertyName("rawResults")]
    public bool RawResults { get; set; }
}