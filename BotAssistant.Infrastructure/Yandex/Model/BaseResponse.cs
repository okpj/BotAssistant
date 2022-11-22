namespace BotAssistant.Infrastructure.Yandex.Model;

public class BaseResponse
{
    [JsonPropertyName("error_code")]
    public string? ErrorCode { get; set; }


    [JsonPropertyName("error_message")]
    public string? ErrorMessage { get; set; }
}
