namespace BotAssistant.Infrastructure.Speech.Modal;

public class RecognizeResult : BaseResponse
{
    [JsonPropertyName("result")]
    public string? Result { get; set; }


    public static RecognizeResult? FromStingJson(string json)
    {
        try
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<RecognizeResult>(json);
            return result;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex.Message);
            return null;
        }
    }
}
