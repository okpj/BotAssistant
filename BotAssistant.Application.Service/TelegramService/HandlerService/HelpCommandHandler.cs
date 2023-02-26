namespace BotAssistant.Application.Service.TelegramService.HandlerService;

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