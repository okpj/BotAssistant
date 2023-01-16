using BotAssistant.Infrastructure.TelegramBot.Model.Constants;

namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers;

public sealed class VoiceMessageHandler : IVoiceMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IYandexSpeechService _yandexSpeechService;
    private readonly IYandexObjectService _yandexObjectService;
    private readonly IObserver<WorkerTask> _recognizeStream;


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
            if (message.Voice.Duration <= DurationConstants.SmallDurationBorder)
                await HandleSmallVoiceMessageAsync(message);
            else if (message.Voice.Duration <= DurationConstants.LongDurationBorder)
                _recognizeStream.OnNext(new WorkerTask { Work = () => HandleLongVoiceMessageAsync(message) });
            else
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, MessageConstants.LongVoiceError, replyToMessageId: message.MessageId);
        }
    }

    private async Task HandleSmallVoiceMessageAsync(Message message)
    {
        var text = await VoiceMessageRecognizeAsync(message!.Voice!);
        if (string.IsNullOrEmpty(text))
            text = MessageConstants.RecognizeError;
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
            var operation = await _yandexSpeechService.StartLongRecognizeTaskAsync(filePath);
            if (operation?.Id is not null)
                await LongVoiceMessageRecognizeAsync(message, operation.Id);
        }
    }

    private async Task LongVoiceMessageRecognizeAsync(Message message, string operationId)
    {
        var result = await _yandexSpeechService.GetLongRecognizeResultAsync(operationId);
        if (result?.Response is not null && result.Done)
        {
            var text = result.Response!.GetFullText().ToString();
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
        }

    }

}
