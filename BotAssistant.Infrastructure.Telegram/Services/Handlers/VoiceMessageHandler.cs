namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers;

public sealed class VoiceMessageHandler : IVoiceMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IYandexSpeechService _yandexSpeechService;
    private readonly IYandexObjectService _yandexObjectService;
    private readonly IObserver<WorkerTask> _recognizeStream;

    private const string _longVoiceError = "Слишком долго говоришь 🥱";
    private const string _recognizeError = "Не удалось распознать 😔";

    private const int SmallDurationBorder = 29;
    private const int LongDurationBorder = 300;

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
            else if (message.Voice.Duration <= LongDurationBorder)
                _recognizeStream.OnNext(new WorkerTask { Work = () => HandleLongVoiceMessageAsync(message) });
            else
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, _longVoiceError, replyToMessageId: message.MessageId);
        }
    }

    private async Task HandleSmallVoiceMessageAsync(Message message)
    {
        var text = await VoiceMessageRecognizeAsync(message!.Voice!);
        if (string.IsNullOrEmpty(text))
            text = _recognizeError;
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
    }

    private async Task<string?> VoiceMessageRecognizeAsync(Voice voice)
    {
        using MemoryStream fileStream = new();
        Telegram.Bot.Types.File voiceFile = await _telegramBotClient.GetInfoAndDownloadFileAsync(voice.FileId, fileStream);
        byte[] voiceByte = fileStream.ToArray();
        RecognizeResult? recognizeResult = await _yandexSpeechService.RecognizeAsync(voiceByte);
        return recognizeResult?.Result;
    }

    private async Task HandleLongVoiceMessageAsync(Message message)
    {
        using MemoryStream fileStream = new();
        Telegram.Bot.Types.File voiceFile = await _telegramBotClient.GetInfoAndDownloadFileAsync(message.Voice!.FileId, fileStream);
        var filePath = voiceFile.GetFileUniqueIdWithOGGExtension();
        var filePutResult = await _yandexObjectService.Put(fileStream, filePath);
        if (filePutResult)
        {
            var operation = await _yandexSpeechService.LongRecognizeAsync(filePath);
            if (operation?.Id is not null)
                await GetLongRecognizeResult(message, operation.Id);
        }
    }

    private async Task GetLongRecognizeResult(Message message, string operationId)
    {
        var result = await _yandexSpeechService.GetLongRecognizeResultAsync(operationId);
        string text = string.Empty;
        if (result?.Response is not null && result.Done) 
            text = result.Response!.GetFullText().ToString();
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
    }

}
