namespace BotAssistant.Infrastructure.Speech.Modal;

public class RecognizeResult : BaseResponse
{
    public string? Result { get; set; }


    public static RecognizeResult? FromStingJson(string json)
    {
        try
        {
            var result = JsonConvert.DeserializeObject<RecognizeResult>(json);
            return result;
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex.Message);
            return null;
        }
    }
}
