namespace BotAssistant.Application.Contract.Model;

public class BaseResult
{
    public string? Error { get; set; }

    public int ErrorCode { get; set; }

    public bool IsSuccess => ErrorCode == 0;
}
