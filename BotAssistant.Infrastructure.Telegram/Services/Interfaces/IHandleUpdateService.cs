namespace BotAssistant.Infrastructure.TelegramBot.Services.Interfaces;

/// <summary>
/// Сервис обработки запросов
/// </summary>
public interface IHandleUpdateService 
{
    /// <summary>
    /// Обработка запроса Update
    /// </summary>
    /// <param name="update">Запрос Update</param>
    /// <returns></returns>
    Task Handle(Update update);
}
