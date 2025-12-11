using TeamChat.Messaging.Contracts.Events;
using TeamChat.Messaging.Contracts.Payload;

namespace TeamChat.Messaging.Contracts.Message
{
    public class MessageCreatedEvent(MessageCreatedPayload payload) : Event<MessageCreatedPayload>(payload)
    {
        public override string EventName => EventNames.Message.MessageCreated;
    }

    public record MessageCreatedPayload(Guid ChatId, Guid MessageId, Guid SenderId, string Content, DateTime SentAt) : BasePayload; 
}