namespace BotAssistant.Application.Contract.YandexCloud;

/// <summary>
/// Сервис токенов Yandex
/// </summary>
public interface IYandexTokenService
{
    /// <summary>
    /// Получить токен
    /// </summary>
    /// <returns></returns>
    Task<IAMToken?> GetToken();
}
