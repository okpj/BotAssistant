﻿using System.Text;

namespace BotAssistant.Infrastructure.Yandex.Model.Extensions;

public static class LongRunningRecognizeResponseExtensions
{
    /// <summary>
    /// Получить полный распознанный текст
    /// </summary>
    /// <param name="recognizeResponse">Ответ на запрос получения результатов длительного распознавания</param>
    /// <returns></returns>
    public static StringBuilder GetFullText(this LongRunningRecognizeResponse recognizeResponse)
    {
        StringBuilder builder = new();
        var alternatives = recognizeResponse.Chunks.SelectMany(x => x.Alternatives).ToArray();
        foreach (var alternative in alternatives)
            builder.AppendLine(alternative.Text);
        return builder;
    }
}
