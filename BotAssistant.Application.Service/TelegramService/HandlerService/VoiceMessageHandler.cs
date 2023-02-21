namespace BotAssistant.Application.Service.TelegramService.HandlerService;

public sealed class VoiceMessageHandler : IVoiceMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IObserver<WorkerTask> _recognizeStream;
    private readonly IVoiceRecognizeService _voiceRecognizeService;

    public VoiceMessageHandler(ITelegramBotClient telegramBotClient,
        IVoiceRecognizeService voiceRecognizeService,
        IObserver<WorkerTask> recognizeStream)
    {
        _telegramBotClient = telegramBotClient;
        _voiceRecognizeService = voiceRecognizeService;
        _recognizeStream = recognizeStream;

    }

    public async Task HandleAsync(Message message)
    {
        if (message.Voice is not null)
        {
            if (message.Voice.Duration <= DurationConstants.SmallDurationBorder)
                await HandleShortVoiceMessageAsync(message);
            else if (message.Voice.Duration <= DurationConstants.LongDurationBorder)
                _recognizeStream.OnNext(new WorkerTask(() => HandleLongVoiceMessageAsync(message)));
            else
                await _telegramBotClient.SendTextMessageAsync(message.Chat.Id,
                    MessageConstants.LongVoiceError, replyToMessageId: message.MessageId);
        }
    }

    private async Task HandleShortVoiceMessageAsync(Message message)
    {
        byte[] voiceByte;
        using (MemoryStream fileStream = new())
        {
            _ = await _telegramBotClient.GetInfoAndDownloadFileAsync(message!.Voice!.FileId, fileStream);
            voiceByte = fileStream.ToArray();
        }

        var recognizeResult = await _voiceRecognizeService.RecognizeShortMassageAsync(voiceByte);
        var text = recognizeResult?.Text;

        if (string.IsNullOrEmpty(text))
            text = MessageConstants.RecognizeError;

        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
    }

    private async Task HandleLongVoiceMessageAsync(Message message)
    {
        using MemoryStream fileStream = new();
        var voiceFile = await _telegramBotClient.GetInfoAndDownloadFileAsync(message.Voice!.FileId, fileStream);

        var filePath = voiceFile.GetFileUniqueIdWithOGGExtension();
        var recognizeResult = await _voiceRecognizeService.RecognizeLongMassageAsync(fileStream, filePath);

        var text = recognizeResult?.Text;

        if (string.IsNullOrEmpty(text))
            text = MessageConstants.RecognizeError;

        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
    }
}
