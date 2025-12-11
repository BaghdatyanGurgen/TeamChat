using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TeamChat.Infrastructure.RabbitMQ;
using TeamChat.Messaging.Contracts.Events;
using TeamChat.Messaging.Contracts.Payload;
using TeamChat.Application.Abstraction.Infrastructure.Messaging;

namespace TeamChat.Infrastructure.Messaging;

public class RabbitMqPublisher : IMessagePublisher
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private IChannel? _channel;

    public RabbitMqPublisher(IOptions<RabbitMQSettings> options)
    {
        var optionsValue = options.Value ?? throw new ArgumentNullException(nameof(options));
        _factory = new ConnectionFactory
        {
            HostName = optionsValue.Host,
            UserName = optionsValue.Username,
            Password = optionsValue.Password,
            Port = optionsValue.Port
        };
    }

    public async Task InitializeAsync()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public async Task PublishAsync<TPayload>(Event<TPayload> mqEvent) where TPayload : BasePayload
    {
        if (_channel is null)
            await InitializeAsync();
        
        await PublishAsync(mqEvent.EventName, mqEvent.Payload);
    }

    private async Task PublishAsync<TPayload>(string routingKey, TPayload message) where TPayload : BasePayload
    {
        if (_channel == null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized. Call InitializeAsync() first.");

        await _channel.QueueDeclareAsync(routingKey, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var a = JsonSerializer.Serialize((object)message);
        Console.WriteLine(a);
        var body = Encoding.UTF8.GetBytes(a);

        await _channel.BasicPublishAsync(
            exchange: "",
            routingKey: routingKey,
            mandatory: false,
            body: body
        );
    }

    public void Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}