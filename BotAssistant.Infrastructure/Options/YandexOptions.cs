namespace BotAssistant.Infrastructure.Options;

public class YandexOptions
{
    public const string ConfigurationSection = "Yandex";

    public string ApiKey { get; set; }

    public string RecognizeURL { get; set; }
}
