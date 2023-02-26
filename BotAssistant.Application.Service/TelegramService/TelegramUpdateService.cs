namespace BotAssistant.Application.Service.TelegramService;

public sealed class TelegramUpdateService : ITelegramUpdateService
{
    private readonly IHelpCommandHandler _helpCommandHandler;
    private readonly IDonateCommandHandler _donateCommandHandler;
    private readonly IVoiceMessageHandler _voiceMessageHandler;

    public TelegramUpdateService(IHelpCommandHandler helpCommandHandler, IVoiceMessageHandler voiceMessageHandler,
        IDonateCommandHandler donateCommandHandler)
    {
        _helpCommandHandler = helpCommandHandler;
        _voiceMessageHandler = voiceMessageHandler;
        _donateCommandHandler = donateCommandHandler;
    }

    public async Task HandleAsync(Update update)
    {
        if (update.PreCheckoutQuery is not null)
            await _donateCommandHandler.AnswerPreCheckoutQuery(update.PreCheckoutQuery);

        if (update.Message is null || update.Message.ViaBot is not null)
            return;

        if (update.Message.Type == MessageType.Voice)
            await _voiceMessageHandler.HandleAsync(update.Message);

        if (update.Message.Entities?.Any(x => x.Type == MessageEntityType.BotCommand) ?? false)
            await CommandHandle(update.Message);
    }

    private async Task CommandHandle(Message message)
    {
        switch (message.Text)
        {
            case Commands.HELP:
                await _helpCommandHandler.HandleAsync(message);
                break;

            case Commands.DONATE:
                await _donateCommandHandler.HandleAsync(message);
                break;

            default:
                break;
        }
    }
}