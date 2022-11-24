namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers;

public sealed class VoiceMessageHandler : IVoiceMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IYandexSpeechService _yandexSpeechService;
    private readonly IYandexObjectService _yandexObjectService;
    private readonly IObserver<WorkerTask> _recognizeStream;

    private const string OGGExtension = ".ogg";
    private const int SmallDurationBorder = 29;
    public VoiceMessageHandler(ITelegramBotClient telegramBotClient,
        IYandexSpeechService yandexSpeechService,
        IYandexObjectService yandexObjectService,
        IObserver<WorkerTask> recognizeStream)
    {
        _telegramBotClient = telegramBotClient;
        _yandexSpeechService = yandexSpeechService;
        _yandexObjectService = yandexObjectService;
        _recognizeStream = recognizeStream;
    }

    public async Task HandleAsync(Message message)
    {
        if (message.Voice is not null)
        {
            if (message.Voice.Duration <= SmallDurationBorder)
                await HandleSmallVoiceMessageAsync(message);
            else
                _recognizeStream.OnNext(new WorkerTask { Work = () => HandleLongVoiceMessageAsync(message) });
        }
    }

    private async Task HandleSmallVoiceMessageAsync(Message message)
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

    private async Task HandleLongVoiceMessageAsync(Message message)
    {
        using MemoryStream fileStream = new();
        Telegram.Bot.Types.File voiceFile = await _telegramBotClient.GetInfoAndDownloadFileAsync(message.Voice!.FileId, fileStream);
        var filePath = $"{voiceFile.FileUniqueId}{OGGExtension}";
        var filePutResult = await _yandexObjectService.Put(fileStream, filePath);
        if (filePutResult)
        {
            var operation = await _yandexSpeechService.LongRecognizeAsync(filePath);
            if (operation?.Id is not null)
            {
                await GetLongRecognizeResult(message, operation.Id);
            };
        }
    }

    private async Task GetLongRecognizeResult(Message message, string operationId)
    {
        var result = await _yandexSpeechService.GetLongRecognizeResultAsync(operationId);
        string text = string.Empty;
        if (result.Done && result.Response is not null)
        {
            text = result.Response!.GetFullText().ToString();
        }
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
    }

}
