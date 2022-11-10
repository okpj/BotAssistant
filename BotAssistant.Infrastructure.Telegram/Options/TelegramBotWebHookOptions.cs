namespace BotAssistant.Infrastructure.TelegramBot.Options;

public class TelegramBotWebHookOptions
{
    public const string ConfigurationSection = "Bot:WebHook";

    public string PathToCert { get; set; }
    public string Name { get; set; }
    public string Token { get; set; }
}
