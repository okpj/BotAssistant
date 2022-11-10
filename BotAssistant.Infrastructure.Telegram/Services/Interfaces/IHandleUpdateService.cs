namespace BotAssistant.Infrastructure.TelegramBot.Services.Interfaces;

public interface IHandleUpdateService 
{
    Task Handle(Update update);
}
