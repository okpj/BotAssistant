using BotAssistant.Application.Contract.YandexCloud.Model.Speech;
using System.Text;

namespace BotAssistant.Util.Extensions;

public static class LongRunningRecognizeResponseExtensions
{
    /// <summary>
    /// Получить полный распознанный текст
    /// </summary>
    /// <param name="recognizeResponse">Ответ на запрос получения результатов длительного распознавания</param>
    /// <returns></returns>
    public static string GetFullText(this LongRunningRecognizeResponse recognizeResponse)
    {
        StringBuilder builder = new();
        var alternatives = recognizeResponse.Chunks.SelectMany(x => x.Alternatives).ToArray();
        foreach (var alternative in alternatives)
            builder.AppendLine(alternative.Text);
        return builder.ToString();
    }
}
