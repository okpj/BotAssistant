using Amazon.S3;
using Amazon.S3.Model;
using System.Net;

namespace BotAssistant.Infrastructure.Yandex.Object;

public sealed class YandexObjectService : IYandexObjectService
{
    private readonly IOptions<YandexOptions> _yandexOptions;
    AmazonS3Client _s3client;
    public YandexObjectService(IOptions<YandexOptions> yandexOptions)
    {
        _yandexOptions = yandexOptions;
        AmazonS3Config configsS3 = new()
        {
            ServiceURL = _yandexOptions.Value.S3ServiceURL,
        };

        _s3client = new(configsS3);
    }

    public async Task<bool> Put(Stream file, string fileName)
    {
        try
        {
            var result = await _s3client.PutObjectAsync(new PutObjectRequest()
            {
                InputStream = file,
                BucketName = _yandexOptions.Value.BucketName,
                Key = fileName,
            });
            if (result.HttpStatusCode == HttpStatusCode.OK)
                return true;
            else
            {
                Serilog.Log.Information("{@ResponseMetadata}, {HttpStatusCode}", result.ResponseMetadata, result.HttpStatusCode);
                return false;
            }
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, nameof(Put));
            return false;
        }
    }
}
