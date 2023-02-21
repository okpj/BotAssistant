namespace BotAssistant.Application.Contract.TelegramServices;
/// <summary>
/// Вебхуки телеграм
/// </summary>
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
