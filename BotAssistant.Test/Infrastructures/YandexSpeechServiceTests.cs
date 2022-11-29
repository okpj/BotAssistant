namespace BotAssistant.Test.Infrastructures;

public class YandexSpeechServiceTests
{
    private readonly YandexOptions _options = new() { ApiKey = "secret-key", RecognizeURL = "https://rec-ya-url.ru" };
    private readonly Mock<IOptions<YandexOptions>> _mockYandexOptions;
    private Mock<HttpMessageHandler>? _mockHttpMessageHandler;



    public YandexSpeechServiceTests()
    {
        _mockYandexOptions = new();
        _mockYandexOptions.Setup(opt => opt.Value).Returns(_options);
    }

    [Fact]
    public async void RecognizeReturnResultString()
    {
        var testResultObject = new RecognizeResult { Result = "ТЕСТ" };
        var content = JsonSerializer.Serialize(testResultObject);

        _mockHttpMessageHandler = new();
        _mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(content)
            });


        var yandexSpeechService = new YandexSpeechService(_mockYandexOptions.Object, new HttpClient(_mockHttpMessageHandler.Object));
        var result = await yandexSpeechService.RecognizeAsync(Array.Empty<byte>());
        Assert.Equal(result?.Result, testResultObject.Result);

    }
}
