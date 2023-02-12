namespace BotAssistant.Application.Service.Telegram.Contracts;

public interface IUpdateService
{
    /// <summary>
    /// Обработка запроса Update
    /// </summary>
    /// <param name="update">Запрос Update</param>
    /// <returns></returns>
    Task Handle(Update update);
}
