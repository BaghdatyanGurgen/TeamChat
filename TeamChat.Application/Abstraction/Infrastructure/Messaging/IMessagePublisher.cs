using TeamChat.Messaging.Contracts.Events;
using TeamChat.Messaging.Contracts.Payload;

namespace TeamChat.Application.Abstraction.Infrastructure.Messaging;

public interface IMessagePublisher : IDisposable
{
    Task PublishAsync<TPayload>(Event<TPayload> mqEvent) where TPayload : BasePayload;
}