namespace BotAssistant.Infrastructure.Options;

public class PaymentOptions
{
    public const string ConfigurationSection = "Payment";

    public string ProviderToken { get; set; }
}
