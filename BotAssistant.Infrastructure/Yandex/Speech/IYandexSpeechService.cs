namespace BotAssistant.Infrastructure.Yandex.Speech;

public interface IYandexSpeechService
{
    Task<RecognizeResult?> RecognizeAsync(byte[] voice);
}
