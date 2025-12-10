using TeamChat.Messaging.Contracts.Payload;

namespace TeamChat.Messaging.Contracts.Events;

public abstract class Event<TPayload>(TPayload payload) where TPayload : BasePayload
{
    public abstract string EventName { get; }
    public BasePayload Payload { get; } = payload;
    public DateTime OccurredAt { get; } = DateTime.UtcNow;
}