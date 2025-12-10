namespace NotificationService.Services;

public class Worker : BackgroundService
{
    private readonly RabbitMqListener _listener;

    public Worker(RabbitMqListener listener)
    {
        _listener = listener;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _listener.StartListening();
        return Task.CompletedTask;
    }
}
