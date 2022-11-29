using BotAssistant.Infrastructure.Yandex.Token.Model;

namespace BotAssistant.Infrastructure.Yandex.Token;

public interface IYandexTokenService
{
    Task<IAMToken?> GetToken();
}
