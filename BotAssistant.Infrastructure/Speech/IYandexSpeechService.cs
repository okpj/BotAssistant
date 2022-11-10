namespace BotAssistant.Infrastructure.Speech;

public interface IYandexSpeechService
{
    Task<RecognizeResult> Recognize(byte[] voice);
}
