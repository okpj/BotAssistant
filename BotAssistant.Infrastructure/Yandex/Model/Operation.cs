namespace BotAssistant.Infrastructure.Yandex.Model;

public class Operation<T> : BaseOperation where T : class
{
    [JsonPropertyName("response")]
    public T? Response { get; set; }
}
