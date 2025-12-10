using System.Text.Json.Serialization;
using TeamChat.Messaging.Contracts.Events;
using TeamChat.Messaging.Contracts.Payload;

namespace TeamChat.Messaging.Contracts.Message
{
    public class MessageCreatedEvent : Event<MessageCreatedPayload>
    {
        public override string EventName => EventNames.Message.MessageCreated;

        public MessageCreatedEvent(MessageCreatedPayload payload) : base(payload) { }
    }
    public class MessageCreatedPayload : BasePayload
    {
        [JsonInclude] public Guid ChatId { get; set; }
        [JsonInclude] public Guid MessageId { get; set; }
        [JsonInclude] public Guid SenderId { get; set; }
        [JsonInclude] public string Content { get; set; }
        [JsonInclude] public DateTime SentAt { get; set; }

        public MessageCreatedPayload(Guid chatId, Guid messageId, Guid senderId, string content, DateTime sentAt)
        {
            ChatId = chatId;
            MessageId = messageId;
            SenderId = senderId;
            Content = content;
            SentAt = sentAt;
        }
    }


}
