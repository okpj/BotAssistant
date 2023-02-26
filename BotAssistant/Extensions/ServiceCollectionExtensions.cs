namespace BotAssistant.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApplicationOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<ApiOptions>(configuration.GetSection(ApiOptions.ConfigurationSection));
        services.Configure<TelegramBotWebHookOptions>(configuration.GetSection(TelegramBotWebHookOptions.ConfigurationSection));
        services.Configure<YandexOptions>(configuration.GetSection(YandexOptions.ConfigurationSection));
        services.Configure<YandexAuthorizedKeyOptions>(configuration.GetSection(YandexAuthorizedKeyOptions.ConfigurationSection));

        services.Configure<PaymentOptions>(configuration.GetSection(PaymentOptions.ConfigurationSection));
    }

    public static void AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<ITelegramBotClient>(new TelegramBotClient(config[$"{TelegramBotWebHookOptions.ConfigurationSection}:Token"]));
        services.AddSingleton<ITelegramWebHook, TelegramWebHook>();
        services.AddSingleton<ITelegramUpdateService, TelegramUpdateService>();

        AddServices(services);
        AddBackgroundServices(services);
    }

    #region private

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IVoiceMessageHandler, VoiceMessageHandler>();
        services.AddSingleton<IHelpCommandHandler, HelpCommandHandler>();
        services.AddSingleton<IDonateCommandHandler, DonateCommandHandler>();

        services.AddSingleton<IVoiceRecognizeService, VoiceRecognizeService>();

        services.AddSingleton<IYandexSpeechService, YandexSpeechService>();
        services.AddSingleton<IYandexTokenService, YandexTokenService>();
        services.AddSingleton<IYandexObjectService, YandexObjectService>();
    }

    private static void AddBackgroundServices(IServiceCollection services)
    {
        services.AddSingleton<ReplaySubject<WorkerTask>>();
        services.AddSingleton<IObservable<WorkerTask>>(x => x.GetRequiredService<ReplaySubject<WorkerTask>>());
        services.AddSingleton<IObserver<WorkerTask>>(x => x.GetRequiredService<ReplaySubject<WorkerTask>>());
        services.AddHostedService<ObserverBackgroundWorker>();
    }

    public static void ConfigureHttpClients(this IServiceCollection services, IConfiguration configuration)
    {
        var yandexApiKey = configuration[$"{YandexOptions.ConfigurationSection}:ApiKey"];

        services.AddHttpClient<IYandexSpeechService, YandexSpeechService>()
            .AddPolicyHandlerFallback()
            .ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Api-Key",
                    yandexApiKey
                );
            });

        services.AddHttpClient<IYandexTokenService, YandexTokenService>()
            .AddPolicyHandlerFallback()
            .ConfigureHttpClient(client =>
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            });
    }

    #endregion private
}