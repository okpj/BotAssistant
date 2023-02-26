namespace BotAssistant.Infrastructure.Options;

/// <summary>
/// Конфигурации Yandex
/// </summary>
public class YandexOptions
{
    public const string ConfigurationSection = "Yandex";

    public string ApiKey { get; set; }

    public string RecognizeURL { get; set; }

    public string LongRecognizeURL { get; set; }

    public string S3ServiceURL { get; set; }

    public string BucketName { get; set; }

    public string StorageUrl { get; set; }

    public string OperationURL { get; set; }
}