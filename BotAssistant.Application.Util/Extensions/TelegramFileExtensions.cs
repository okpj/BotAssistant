namespace BotAssistant.Application.Util.Extensions;

public static class TelegramFileExtensions
{
    private const string OGGExtension = ".ogg";

    public static string GetFileUniqueIdWithOGGExtension(this Telegram.Bot.Types.File voiceFile)
        => $"{voiceFile.FileUniqueId}{OGGExtension}";
}