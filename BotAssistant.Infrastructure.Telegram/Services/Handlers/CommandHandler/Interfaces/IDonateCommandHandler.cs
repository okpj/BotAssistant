using Telegram.Bot.Types.Payments;

namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers.CommandHandler.Interfaces;

/// <summary>
/// Обработчик команды Donate
/// </summary>
public interface IDonateCommandHandler: IHandler
{
    Task AnswerPreCheckoutQuery(PreCheckoutQuery preCheckoutQuery);
}
