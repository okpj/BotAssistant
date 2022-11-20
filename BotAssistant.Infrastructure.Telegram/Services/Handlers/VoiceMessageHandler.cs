namespace BotAssistant.Infrastructure.TelegramBot.Services.Handlers;

public sealed class VoiceMessageHandler : IVoiceMessageHandler
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IYandexSpeechService _yandexSpeechService;
    public VoiceMessageHandler(ITelegramBotClient telegramBotClient, IYandexSpeechService yandexSpeechService)
    {
        _telegramBotClient = telegramBotClient;
        _yandexSpeechService = yandexSpeechService;
    }

    public async Task Handle(Message message)
    {
        if (message.Voice is not null)
        {
            var text = await VoiceMessageRecognize(message!.Voice!);
            if (string.IsNullOrEmpty(text))
                text = "Не удалось распознать :(";
            await _telegramBotClient.SendTextMessageAsync(message.Chat.Id, text, replyToMessageId: message.MessageId);
        }
    }


    private async Task<string?> VoiceMessageRecognize(Voice voice)
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
