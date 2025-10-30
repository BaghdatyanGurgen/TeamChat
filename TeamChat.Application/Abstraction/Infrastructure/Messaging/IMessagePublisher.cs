using TeamChat.Messaging.Contracts.Events;

namespace TeamChat.Application.Abstraction.Infrastructure.Messaging
{
    public interface IMessagePublisher : IDisposable
    {
        Task PublishAsync<T>(T mqEvent) where T : Event;
    }
}