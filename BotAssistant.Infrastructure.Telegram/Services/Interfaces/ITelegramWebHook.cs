namespace BotAssistant.Infrastructure.TelegramBot.Services.Interfaces;
public interface ITelegramWebHook
{
    /// <summary>
    /// Установить вебхук
    /// </summary>
    /// <returns></returns>
    Task SetAsync();

    /// <summary>
    /// Удалить действующие вебхуки
    /// </summary>
    /// <returns></returns>
    Task DeleteAsync();
}
