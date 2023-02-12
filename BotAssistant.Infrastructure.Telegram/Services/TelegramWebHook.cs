namespace BotAssistant.Infrastructure.TelegramBot.Services;

public class TelegramWebHook : ITelegramWebHook
{
    private readonly ITelegramBotClient _telegramBotClient;
    private readonly IOptions<TelegramBotWebHookOptions> _telegramBotWebHookOptions;
    private readonly IOptions<ApiOptions> _apiOptions;

    public TelegramWebHook(ITelegramBotClient telegramBotClient,
        IOptions<TelegramBotWebHookOptions> telegramBotWebHookOptions, IOptions<ApiOptions> apiOptions)
    {
        _telegramBotClient = telegramBotClient;
        _telegramBotWebHookOptions = telegramBotWebHookOptions;
        _apiOptions = apiOptions;

    }

    public async Task SetAsync()
    {
        try
        {
            if (string.IsNullOrEmpty(_telegramBotWebHookOptions.Value.PathToCert) is false)
            {
                await using var fs = File.OpenRead(_telegramBotWebHookOptions.Value.PathToCert);
                InputFileStream cert = new(fs);
                await _telegramBotClient.SetWebhookAsync
                    ($"{_apiOptions.Value.BaseUrl}/api/telegram/update?authToken={_apiOptions.Value.AuthToken}", cert);
            }
            else
            {
                await _telegramBotClient.SetWebhookAsync
                    ($"{_apiOptions.Value.BaseUrl}/api/Telegram/Update?authToken={_apiOptions.Value.AuthToken}");
            }
        }
        finally
        {
            var info = await _telegramBotClient.GetWebhookInfoAsync();
            Log.Information("Webhook Info : {@Info}", info);
        }
    }

    public async Task DeleteAsync()
    {
        await _telegramBotClient.DeleteWebhookAsync();
    }

}
