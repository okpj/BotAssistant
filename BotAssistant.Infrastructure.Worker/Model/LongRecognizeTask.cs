namespace BotAssistant.Infrastructure.Worker.Model;

public class WorkerTask
{
    public Func<Task> Work{ get; set; }

}
