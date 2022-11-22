namespace BotAssistant.Infrastructure.Yandex.Speech;

public class YandexSpeechService : IYandexSpeechService
{
    private readonly IOptions<YandexOptions> _yandexOptions;
    private readonly HttpClient _httpClient;
    public YandexSpeechService(IOptions<YandexOptions> yandexOptions, HttpClient httpClient)
    {
        _yandexOptions = yandexOptions;
        _httpClient = httpClient;
    }

    public async Task<RecognizeResult?> RecognizeAsync(byte[] voice)
    {
        try
        {
            using (var content = new ByteArrayContent(voice))
            {
                var reqUri = new Uri(_yandexOptions.Value.RecognizeURL);
                var response = await _httpClient.PostAsync(reqUri, content);
                var resultResponse = await response.Content.ReadAsStringAsync();
                return RecognizeResult.FromStingJson(resultResponse);
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, nameof(RecognizeAsync));
            return new RecognizeResult { ErrorCode = "-1", ErrorMessage = "Ошибка при выполнении распознавания" };
        }
    }
}
