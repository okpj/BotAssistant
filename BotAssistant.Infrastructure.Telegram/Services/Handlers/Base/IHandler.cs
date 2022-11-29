namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers.Base;

public interface IHandler
{
    Task HandleAsync(Message message);
}
