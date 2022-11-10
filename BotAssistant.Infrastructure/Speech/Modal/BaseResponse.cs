namespace BotAssistant.Infrastructure.Speech.Modal;

public class BaseResponse
{
    [JsonProperty("error_code")]
    public string? ErrorCode { get; set; }


    [JsonProperty("error_message")]
    public string? ErrorMessage { get; set; }
}
