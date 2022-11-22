namespace BotAssistant.Infrastructure.Yandex.Object;

public interface IYandexObjectService
{
    Task<bool> Put(Stream file, string fileName);
}
