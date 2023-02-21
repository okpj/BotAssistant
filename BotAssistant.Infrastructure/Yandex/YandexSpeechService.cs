namespace BotAssistant.Infrastructure.Yandex;

public class YandexSpeechService : IYandexSpeechService
{
    private readonly IOptions<YandexOptions> _yandexOptions;
    private readonly HttpClient _httpClient;
    private const int RequestNumberForLongRecognize = 40;

    public YandexSpeechService(IOptions<YandexOptions> yandexOptions, HttpClient httpClient)
    {
        _yandexOptions = yandexOptions;
        _httpClient = httpClient;
    }

    public async Task<RecognizeResponse?> RecognizeAsync(byte[] voice)
    {
        try
        {
            using (var content = new ByteArrayContent(voice))
            {
                var reqUri = new Uri(_yandexOptions.Value.RecognizeURL);
                var response = await _httpClient.PostAsync(reqUri, content);

                if (response.IsSuccessStatusCode)
                {
                    var resultResponse = await response.Content.ReadAsStringAsync();
                    return JsonHelper.FromStingJson<RecognizeResponse>(resultResponse);
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(RecognizeAsync));
        }

        return null;
    }

    public async Task<BaseOperation?> StartLongRecognizeTaskAsync(string filePath)
    {
        try
        {
            LongRunningRecognizeRequest request = new()
            {
                Config = new() { Specification = new Specification(true, true) },
                Audio = new Audio($"{_yandexOptions.Value.StorageUrl}/{_yandexOptions.Value.BucketName}/{filePath}")
            };
            var jsonContent = System.Text.Json.JsonSerializer.Serialize(request);
            var reqUri = new Uri(_yandexOptions.Value.LongRecognizeURL);
            var response = await _httpClient.PostAsync(reqUri, new StringContent(jsonContent));

            if (response.IsSuccessStatusCode)
            {
                var resultResponse = await response.Content.ReadAsStringAsync();
                return JsonHelper.FromStingJson<BaseOperation>(resultResponse);
            }

        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(StartLongRecognizeTaskAsync));
        }
        return null;
    }

    public async Task<RecognizeResponse?> GetLongRecognizeResultAsync(string operationId)
    {
        var reqUri = new Uri($"{_yandexOptions.Value.OperationURL}/{operationId}");
        Operation<LongRunningRecognizeResponse>? operation = null;

        try
        {
            HttpResponseMessage? response = null;
            string? resultResponse = null;
            var countRequests = 0;
            while (RequestNumberForLongRecognize > countRequests)
            {
                countRequests++;
                await Task.Delay(2000);
                response = await _httpClient.GetAsync(reqUri);
                resultResponse = await response.Content.ReadAsStringAsync();
                operation = JsonHelper.FromStingJson<Operation<LongRunningRecognizeResponse>>(resultResponse);
                var operationIsDone = operation?.Done ?? false;
                if (operationIsDone || response.IsSuccessStatusCode is false)
                    break;
            }
            var operationResponse = operation?.Response;
            if (operationResponse?.Chunks is not null)
            {
                return new RecognizeResponse
                {
                    Result = operationResponse.GetFullText()
                };
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(GetLongRecognizeResultAsync));
        }
        return null;
    }
}
