namespace NotificationService.Services;

public class Worker : BackgroundService
{
    private readonly RabbitMqListener _listener;
    private readonly ILogger<Worker> _logger;

    public Worker(RabbitMqListener listener, ILogger<Worker> logger)
    {
        _listener = listener;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            _logger.LogInformation("Notification Worker started");
            await _listener.StartListening(stoppingToken);
        }
        catch (OperationCanceledException)
        {
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Notification Worker failed");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Notification Worker stopping");
        await _listener.DisposeAsync();
        await base.StopAsync(cancellationToken);
    }
}
