using BotAssistant.Infrastructure.Yandex.Model.Speech;

namespace BotAssistant.Infrastructure.Yandex.Speech;

public interface IYandexSpeechService
{
    Task<RecognizeResult?> RecognizeAsync(byte[] voice);

    Task<BaseOperation?> LongRecognizeAsync(string filePath);

    Task<Operation<LongRunningRecognizeResponse>> GetLongRecognizeResultAsync(string operationId);
}
