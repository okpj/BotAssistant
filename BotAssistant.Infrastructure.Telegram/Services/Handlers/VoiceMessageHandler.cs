namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers;

public sealed class VoiceMessageHandler : IVoiceMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IYandexSpeechService _yandexSpeechService;
    private readonly IYandexObjectService _yandexObjectService;
    
    private const string OGGExtension = ".ogg";
    private const int SmallDurationBorder = 29;
    public VoiceMessageHandler(ITelegramBotClient telegramBotClient,
        IYandexSpeechService yandexSpeechService,
        IYandexObjectService yandexObjectService)
    {
        _telegramBotClient = telegramBotClient;
        _yandexSpeechService = yandexSpeechService;
        _yandexObjectService = yandexObjectService;
    }

    public async Task HandleAsync(Message message)
    {
        if (message.Voice is not null)
        {
            if (message.Voice.Duration <= SmallDurationBorder)
                await HandlerSmallVoiceMessageAsync(message);
            else
                await HandlerLongVoiceMessageAsync(message);
        }
    }

    private async Task HandlerSmallVoiceMessageAsync(Message message)
    {
        var text = await VoiceMessageRecognizeAsync(message!.Voice!);
        if (string.IsNullOrEmpty(text))
            text = "Не удалось распознать :(";
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
    }

    private async Task<string?> VoiceMessageRecognizeAsync(Voice voice)
    {
        using MemoryStream fileStream = new();
        Telegram.Bot.Types.File voiceFile = await _telegramBotClient.GetInfoAndDownloadFileAsync(voice.FileId, fileStream);
        byte[] voiceByte = fileStream.ToArray();
        RecognizeResult? result = await _yandexSpeechService.RecognizeAsync(voiceByte);
        return result?.Result ?? result?.ErrorMessage;
    }

    private async Task HandlerLongVoiceMessageAsync(Message message)
    {
        using MemoryStream fileStream = new();
        Telegram.Bot.Types.File voiceFile = await _telegramBotClient.GetInfoAndDownloadFileAsync(message.Voice!.FileId, fileStream);
        var filePutResult = await _yandexObjectService.Put(fileStream, $"{voiceFile.FileUniqueId}{OGGExtension}");
    }

}
