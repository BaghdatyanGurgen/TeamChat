using TeamChat.Messaging.Contracts.Payload;

namespace TeamChat.Messaging.Contracts.Events;

public abstract class Event(BasePayload payload)
{
    public abstract string EventName { get; }
    public BasePayload Payload { get; } = payload;
}