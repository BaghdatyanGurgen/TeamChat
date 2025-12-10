using Microsoft.Extensions.Options;
using NotificationService.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NotificationService.Services;

public class RabbitMqListener : IDisposable
{
    private IConnection _connection;
    private IChannel _channel;
    private readonly ConnectionFactory _factory;
    private readonly string _queueName;

    public RabbitMqListener(IOptions<RabbitMQSettings> options)
    {
        var settings = options.Value ?? throw new ArgumentNullException(nameof(options));
        _queueName = settings.QueueName;

        _factory = new ConnectionFactory
        {
            HostName = settings.Host,
            Port = settings.Port,
            UserName = settings.Username,
            Password = settings.Password
        };
    }

    private async Task InitConnection()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
        await _channel.QueueDeclareAsync(queue: _queueName, durable: true, exclusive: false, autoDelete: false);

    }

    public async Task StartListening()
    {
        await InitConnection();
        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var messageJson = Encoding.UTF8.GetString(body);

            var payload = JsonSerializer.Deserialize<MessageCreatedPayload>(messageJson);
            Console.WriteLine($"[NotificationService] Received: {messageJson}");

            await _channel.BasicAckAsync(ea.DeliveryTag, false);
            await Task.Yield();
        };

        await _channel.BasicConsumeAsync(queue: _queueName, autoAck: false, consumer: consumer);
        Console.WriteLine($"[NotificationService] Listening on queue '{_queueName}'...");
    }

    public void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}
