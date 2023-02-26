namespace BotAssistant.Infrastructure.Options;

/// <summary>
/// Конфигурации платёжного шлюза
/// </summary>
public class PaymentOptions
{
    public const string ConfigurationSection = "Payment";

    public string ProviderToken { get; set; }
}