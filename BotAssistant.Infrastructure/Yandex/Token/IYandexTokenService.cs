using BotAssistant.Infrastructure.Yandex.Token.Model;

namespace BotAssistant.Infrastructure.Yandex.Token;

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
