namespace BotAssistant.Application.Contract.TelegramServices.Base;
public interface IHandler
{
    /// <summary>
    /// Обработчик сообщения
    /// </summary>
    /// <param name="message">Сообщение</param>
    /// <returns></returns>
    Task HandleAsync(Message message);
}
