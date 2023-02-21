using Serilog;

namespace BotAssistant.Application.Util.Helpers;

public class JsonHelper
{
    public static T? FromStingJson<T>(string json)
    {
        try
        {
            var result = System.Text.Json.JsonSerializer.Deserialize<T>(json);
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex.Message);
            return default;
        }
    }
}
