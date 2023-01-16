namespace BotAssistant.Infrastructure.TelegramBot.Model.Constants;

/// <summary>
/// Константы сообщений
/// </summary>
public class MessageConstants
{
    /// <summary>
    /// Ошибка для слишком длинных сообщений
    /// </summary>
    public const string LongVoiceError = "Слишком долго говоришь 🥱";

    /// <summary>
    /// Ошибка распознавания
    /// </summary>
    public const string RecognizeError = "Не удалось распознать 😔";

    /// <summary>
    /// Справка
    /// </summary>

    public const string HelpDescription = "Для распознавания речи, отправьте боту или перешлите из другого чата аудио-сообщение 🔊";
}
