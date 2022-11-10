namespace BotAssistant.Infrastructure.TelegramBot.Services;

public class HandleUpdateService : IHandleUpdateService
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IYandexSpeechService _yandexSpeechService;
    public HandleUpdateService(ITelegramBotClient telegramBotClient, IYandexSpeechService yandexSpeechService)
    {
        _telegramBotClient = telegramBotClient;
        _yandexSpeechService = yandexSpeechService;

    }
    public async Task Handle(Update update)
    {
        if (update.Message is null || update.Message.ForwardFrom is not null)
            return;

        if (update.Message?.Voice is not null)
            await VoiceHandle(update.Message);
    }

    public async Task VoiceHandle(Message message)
    {
        var text = await VoiceMessageRecognize(message!.Voice!);
        if (string.IsNullOrEmpty(text))
            text = "Не удалось распознать :(";
        await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
    }

    public async Task<string?> VoiceMessageRecognize(Voice voice)
    {
        if (voice.Duration <= 29)
        {
            using (MemoryStream fileStream = new())
            {
                Telegram.Bot.Types.File voiceFile = await _telegramBotClient.GetInfoAndDownloadFileAsync(voice.FileId, fileStream);
                var voiceByte = fileStream.ToArray();
                var result = await _yandexSpeechService.Recognize(voiceByte);
                return result.Result ?? result.ErrorMessage;
            }
        }
        Log.Warning($"The voice message is too long: {voice.Duration} сек.");
        return null;
    }

}
