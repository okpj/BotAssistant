using BotAssistant.Infrastructure.Worker.Model;
using Microsoft.Extensions.Hosting;

namespace BotAssistant.Infrastructure.Worker;

public class ObserverBackgroundWorker : BackgroundService
{
    private readonly IObservable<WorkerTask> _recognizeStream;
    private IDisposable? _subscription;
    public ObserverBackgroundWorker(IObservable<WorkerTask> recognizeStream)
    {
        _recognizeStream = recognizeStream;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _subscription = _recognizeStream.Subscribe(async item => await item.Work());
        return Task.CompletedTask;
    }
}
