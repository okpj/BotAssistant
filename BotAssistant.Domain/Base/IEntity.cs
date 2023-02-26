namespace BotAssistant.Domain.Base;

/// <summary>
/// Базовая сущность
/// </summary>
/// <typeparam name="TKey">Тип данных для первичного ключа</typeparam>
public interface IEntity<TKey> where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    TKey Id { get; set; }
}