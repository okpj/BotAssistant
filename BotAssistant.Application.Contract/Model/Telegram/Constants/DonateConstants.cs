namespace BotAssistant.Application.Contract.Model.Telegram.Constants;

/// <summary>
/// Константы для донатов
/// </summary>
public static class DonateConstants
{
    /// <summary>
    /// Валюта
    /// </summary>
    public const string Currency = "RUB";

    /// <summary>
    /// Заголовок платежа
    /// </summary>
    public const string Title = "Пожертвование";

    /// <summary>
    /// Описание платежа
    /// </summary>
    public const string Descriptions = "На развитие проекта";

    /// <summary>
    /// Сумма платежа (в копейках)
    /// </summary>
    public const int AmountInMinimalCurrencyUnits = 100 * 100;
}