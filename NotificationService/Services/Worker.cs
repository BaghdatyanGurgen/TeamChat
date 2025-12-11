namespace NotificationService.Services;

public class Worker : BackgroundService
{
    private readonly RabbitMqListener _listener;

    public Worker(RabbitMqListener listener)
    {
        _listener = listener;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        => await _listener.StartListening();
        
}
