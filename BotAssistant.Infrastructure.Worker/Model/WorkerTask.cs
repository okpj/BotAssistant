namespace BotAssistant.Infrastructure.Worker.Model;

/// <summary>
/// Задача для фоновой обработки
/// </summary>
public class WorkerTask
{
    public WorkerTask() { }

    /// <summary>
    /// Конструктор WorkerTask
    /// </summary>
    /// <param name="work">Выполняемая работа (задача)</param>
    public WorkerTask(Func<Task> work)
    {
        Work = work;
    }

    /// <summary>
    /// Выполняемая работа (задача)
    /// </summary>
    public Func<Task> Work { get; set; }

}
