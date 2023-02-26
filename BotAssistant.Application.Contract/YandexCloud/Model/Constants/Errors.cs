namespace BotAssistant.Application.Contract.YandexCloud.Model.Constants;

/// <summary>
/// Ошибки сервиса распознавания
/// </summary>
public static class RecognizeErrors
{
    public struct RecognizeError
    {
        public const string? ErrorCode = "-1";
        public const string? ErrorMessage = "Ошибка при выполнении запроса распознавания";
    }

    public struct LongRecognizeError
    {
        public const string? ErrorCode = "-2";
        public const string? ErrorMessage = "Ошибка при выполнении запроса длительного распознавания";
    }
}