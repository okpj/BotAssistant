using BotAssistant.Infrastructure.Yandex.Model.Constants;

namespace BotAssistant.Infrastructure.Yandex.Speech;

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
                    LogResponse(nameof(StartLongRecognizeTaskAsync), resultResponse);

                return JsonHelper.FromStingJson<RecognizeResult>(resultResponse);
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(RecognizeAsync));
            return new RecognizeResult
            {
                ErrorCode = RecognizeErrors.RecognizeError.ErrorCode,
                ErrorMessage = RecognizeErrors.RecognizeError.ErrorMessage
            };
        }
    }

    public async Task<BaseOperation?> StartLongRecognizeTaskAsync(string filePath)
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
                LogResponse(nameof(StartLongRecognizeTaskAsync), resultResponse);

            var resultLongRecognize = JsonHelper.FromStingJson<BaseOperation>(resultResponse);
            return resultLongRecognize;

        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(StartLongRecognizeTaskAsync));
            return new BaseOperation
            {
                ErrorCode = RecognizeErrors.LongRecognizeError.ErrorCode,
                ErrorMessage = RecognizeErrors.LongRecognizeError.ErrorMessage
            };
        }
    }

    public async Task<Operation<LongRunningRecognizeResponse>?> GetLongRecognizeResultAsync(string operationId)
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
                await Task.Delay(1500);
                response = await _httpClient.GetAsync(reqUri);
                resultResponse = await response.Content.ReadAsStringAsync();
                operation = JsonHelper.FromStingJson<Operation<LongRunningRecognizeResponse>>(resultResponse);
                if (operation?.Done ?? false || response.IsSuccessStatusCode is false)
                    break;
            }

            if (response?.IsSuccessStatusCode is false)
                LogResponse(nameof(GetLongRecognizeResultAsync), resultResponse);
            return operation;
        }
        catch (Exception ex)
        {
            Log.Error(ex, nameof(GetLongRecognizeResultAsync));
            return new Operation<LongRunningRecognizeResponse>
            {
                ErrorCode = RecognizeErrors.LongRecognizeError.ErrorCode,
                ErrorMessage = RecognizeErrors.LongRecognizeError.ErrorMessage
            };
        }
    }

    private static void LogResponse(string methodName, string? resultResponse)
    {
        Log.Information("Error in {name}: {@resultResponse}", methodName, resultResponse);
    }
}
