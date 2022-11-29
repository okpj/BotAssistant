using Telegram.Bot.Types.Payments;

namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers.CommandHandler.Interfaces;

public interface IDonateCommandHandler: IHandler
{
    Task AnswerPreCheckoutQuery(PreCheckoutQuery preCheckoutQuery);
}
