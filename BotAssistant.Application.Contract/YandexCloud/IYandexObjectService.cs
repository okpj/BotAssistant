namespace BotAssistant.Application.Contract.YandexCloud;

/// <summary>
/// Сервис для работы с файловым хранилищем Yandex (Bucket)
/// </summary>
public interface IYandexObjectService
{
    /// <summary>
    /// Загрузить файл в хранилище
    /// </summary>
    /// <param name="file">Файл</param>
    /// <param name="fileName">Название файла</param>
    /// <returns></returns>
    Task<bool> Put(Stream file, string fileName);
}
