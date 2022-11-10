namespace BotAssistant.Infrastructure.Options;

public class ApiOptions
{
    public const string ConfigurationSection = "Api";

    public string AuthToken { get; set; }
    public string? BaseUrl { get; set; }
}
