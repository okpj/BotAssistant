namespace BotAssistant.Infrastructure.TelegramBot.Extensions;

public static class FileExtensions
{
    private const string OGGExtension = ".ogg";
    public static string GetFileUniqueIdWithOGGExtension(this Telegram.Bot.Types.File voiceFile)
        => $"{voiceFile.FileUniqueId}{OGGExtension}";
}
