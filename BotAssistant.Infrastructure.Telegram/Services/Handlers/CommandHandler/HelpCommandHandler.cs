namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers.CommandHandler;

public sealed class HelpCommandHandler : IHelpCommandHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private const string Description = "Для распознавания речи, отправьте боту или перешлите из другого чата аудио-сообщение";
    public HelpCommandHandler(ITelegramBotClient telegramBotClient)
    {
        _telegramBotClient = telegramBotClient;
    }

    public async Task Handle(Message message)
    {
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, Description) ;
    }

}
