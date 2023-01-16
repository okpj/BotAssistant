namespace BotAssistant.Infrastructure.Options;

/// <summary>
/// Конфигурации API
/// </summary>
public class ApiOptions
{
    public const string ConfigurationSection = "Api";

    public string AuthToken { get; set; }
    public string? BaseUrl { get; set; }
}
