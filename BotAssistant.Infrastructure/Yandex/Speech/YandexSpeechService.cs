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

                if (response.IsSuccessStatusCode is false)
                    LogResponse(nameof(LongRecognizeAsync), resultResponse);

                return JsonHelper.FromStingJson<RecognizeResult>(resultResponse);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(RecognizeAsync));
            return new RecognizeResult { ErrorCode = "-1", ErrorMessage = "Ошибка при выполнении распознавания" };
        }
    }

    public async Task<BaseOperation?> LongRecognizeAsync(string filePath)
    {
        try
        {
            LongRunningRecognizeRequest request = new()
            {
                Config = new()
                {
                    Specification = new() { LiteratureText = true, RawResults = true }
                },
                Audio = new()
                {
                    Uri = $"{_yandexOptions.Value.StorageUrl}/{_yandexOptions.Value.BucketName}/{filePath}"
                }
            };
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(request);
            var reqUri = new Uri(_yandexOptions.Value.LongRecognizeURL);
            var response = await _httpClient.PostAsync(reqUri, new StringContent(jsonContent));
            var resultResponse = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode is false)
                LogResponse(nameof(LongRecognizeAsync), resultResponse);

            var resultLongRecognize = JsonHelper.FromStingJson<BaseOperation>(resultResponse);
            return resultLongRecognize;

        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(LongRecognizeAsync));
            return new BaseOperation { ErrorCode = "-1", ErrorMessage = "Ошибка при выполнении запроса длительного распознавания" };
        }
    }

    public async Task<Operation<LongRunningRecognizeResponse>?> GetLongRecognizeResultAsync(string operationId)
    {
        var reqUri = new Uri($"{_yandexOptions.Value.OperationURL}/{operationId}");
        Operation<LongRunningRecognizeResponse>? operation = null;
        int requestNumber = 40;
        try
        {
            HttpResponseMessage? response = null;
            string? resultResponse = null;
            while (requestNumber > 0)
            {
                await Task.Delay(1500);
                response = await _httpClient.GetAsync(reqUri);
                resultResponse = await response.Content.ReadAsStringAsync();
                operation = JsonHelper.FromStingJson<Operation<LongRunningRecognizeResponse>>(resultResponse);
                if (operation?.Done ?? false || response.IsSuccessStatusCode is false)
                    break;
            }

            if (response?.IsSuccessStatusCode is false)
                LogResponse(nameof(LongRecognizeAsync), resultResponse);
            return operation;
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(GetLongRecognizeResultAsync));
            return new Operation<LongRunningRecognizeResponse> { ErrorCode = "-1", ErrorMessage = "Ошибка при выполнении запроса длительного распознавания" };
        }
    }

    private void LogResponse(string methodName, string? resultResponse)
    {
        Log.Information("Error in {name}: {@resultResponse}", methodName, resultResponse);
    }
}
