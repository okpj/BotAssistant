using BotAssistant.Infrastructure.TelegramBot.Model.Constants;

namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers.CommandHandler;

public sealed class HelpCommandHandler : IHelpCommandHandler
{
    private readonly ITelegramBotClient _telegramBotClient;

    public HelpCommandHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task HandleAsync(Message message)
    {
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, MessageConstants.HelpDescription);
    }

}
