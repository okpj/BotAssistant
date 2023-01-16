namespace BotAssistant.Infrastructure.Worker.Model;

/// <summary>
/// Задача для фоновой обработки
/// </summary>
public class WorkerTask
{
    /// <summary>
    /// Выполняемая работа (задача)
    /// </summary>
    public Func<Task> Work{ get; set; }

}
