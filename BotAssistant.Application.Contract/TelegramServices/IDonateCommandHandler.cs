namespace BotAssistant.Application.Contract.TelegramServices;

public interface IDonateCommandHandler : IHandler {
    Task AnswerPreCheckoutQuery(PreCheckoutQuery preCheckoutQuery);
}
