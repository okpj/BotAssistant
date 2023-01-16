﻿namespace BotAssistant.Infrastructure.Yandex.Speech;

/// <summary>
/// Сервис распознавания сообщений
/// </summary>
public interface IYandexSpeechService
{
    /// <summary>
    /// Распознавание коротких сообщений (до 30 сек)
    /// </summary>
    /// <param name="voice">Аудио файл</param>
    /// <returns></returns>
    Task<RecognizeResult?> RecognizeAsync(byte[] voice);

    /// <summary>
    /// Запуск операции распознавания длительных сообщений
    /// </summary>
    /// <param name="filePath">Путь до аудио-файла в хранилище</param>
    /// <returns></returns>
    Task<BaseOperation?> StartLongRecognizeTaskAsync(string filePath);

    /// <summary>
    /// Получение результата операции длительного распознавания
    /// </summary>
    /// <param name="operationId">Идентификатор операции полученный при запуске операции длительного распознавания</param>
    /// <returns></returns>
    Task<Operation<LongRunningRecognizeResponse>?> GetLongRecognizeResultAsync(string operationId);
}
