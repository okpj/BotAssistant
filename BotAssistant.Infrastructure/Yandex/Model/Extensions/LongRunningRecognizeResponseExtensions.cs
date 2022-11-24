using BotAssistant.Infrastructure.Yandex.Model.Speech;
using System.Text;

namespace BotAssistant.Infrastructure.Yandex.Model.Extensions;

public static class LongRunningRecognizeResponseExtensions
{
    public static StringBuilder GetFullText(this LongRunningRecognizeResponse recognizeResponse)
    {
        StringBuilder builder = new();
        var alternatives = recognizeResponse.Chunks.SelectMany(x => x.Alternatives).ToArray();
        foreach (var alternative in alternatives)
            builder.AppendLine(alternative.Text);
        return builder;
    }
}
