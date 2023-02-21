namespace BotAssistant.Application.Contract.TelegramServices;

public interface ITelegramUpdateService
{
    /// <summary>
    /// Обработка запроса Update
    /// </summary>
    /// <param name="update">Запрос Update</param>
    /// <returns></returns>
    Task HandleAsync(Update update);
}

