namespace BotAssistant.Infrastructure.Options;

public class YandexAuthorizedKeyOptions
{
    public const string ConfigurationSection = "YandexAuthorizedKeys";

    public string Id { get; set; }

    public string ServiceAccountId { get; set; }

    public string CreatedAt { get; set; }

    public string KeyAlgorithm { get; set; }

    public string PublicKey { get; set; }

    public string PrivateKey { get; set; }

    public string TokenUrl { get; set; }
}
