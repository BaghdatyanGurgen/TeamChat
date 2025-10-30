using System.Text;
using RabbitMQ.Client;
using System.Text.Json;
using Microsoft.Extensions.Options;
using TeamChat.Infrastructure.RabbitMQ;
using TeamChat.Application.Abstraction.Infrastructure.Messaging;
using TeamChat.Messaging.Contracts.Events;

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
            Password = optionsValue.Host,
            Port = optionsValue.Port
        };
    }

    public async Task InitializeAsync()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();
    }

    public async Task PublishAsync<T>(T mqEvent) where T : Event
    {
        await PublishAsync(mqEvent.EventName, mqEvent);
    }


    private async Task PublishAsync(string routingKey, Event message) 
    {
        if (_channel == null)
            throw new InvalidOperationException("RabbitMQ channel is not initialized. Call InitializeAsync() first.");

        await _channel.QueueDeclareAsync(routingKey, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

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