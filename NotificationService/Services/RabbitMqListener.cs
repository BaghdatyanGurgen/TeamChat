using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using RabbitMQ.Client.Events;
using NotificationService.Models;
using Microsoft.Extensions.Options;

namespace NotificationService.Services;

public class RabbitMqListener : IAsyncDisposable
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;
    private readonly ILogger<RabbitMqListener> _logger;

    public RabbitMqListener(IOptions<RabbitMQSettings> options, ILogger<RabbitMqListener> logger)
    {
        var settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        _queueName = settings.QueueName;
        _logger = logger;

        _factory = new ConnectionFactory
        {
            HostName = settings.Host,
            Port = settings.Port,
            UserName = settings.Username,
            Password = settings.Password
        };
    }

    private async Task InitConnectionAsync()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        _logger.LogInformation("RabbitMQ connection initialized for queue '{QueueName}'", _queueName);
    }

    public async Task StartListening(CancellationToken stoppingToken = default)
    {
        await InitConnectionAsync();

        if (_channel is null)
            throw new InvalidOperationException("Channel is not initialized");

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var messageJson = Encoding.UTF8.GetString(body);

                var payload = JsonSerializer.Deserialize<MessageCreatedPayload>(messageJson);
                if (payload != null)
                {
                    _logger.LogInformation(
                        "Received MessageCreated: {MessageId} from Chat {ChatId}",
                        payload.MessageId, payload.ChatId
                    );

                }

                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process message from queue '{QueueName}'", _queueName);
            }

            await Task.Yield();
        };

        await _channel.BasicConsumeAsync(
            queue: _queueName,
            autoAck: false,
            consumer: consumer
        );

        _logger.LogInformation("Listening on queue '{QueueName}'...", _queueName);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel != null)
        {
            await _channel.CloseAsync();
            await _channel.DisposeAsync();
        }

        if (_connection != null)
        {
            await _connection.CloseAsync();
            await _connection.DisposeAsync();
        }

        _logger.LogInformation("RabbitMQ connection disposed");
    }
}