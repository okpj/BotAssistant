namespace BotAssistant.Application.Contract.YandexCloud.Model;

/// <summary>
/// Операция конкретного типа
/// </summary>
/// <typeparam name="T">Тип операции</typeparam>
public class Operation<T> : BaseOperation where T : class
{
    [JsonPropertyName("response")]
    public T? Response { get; set; }
}