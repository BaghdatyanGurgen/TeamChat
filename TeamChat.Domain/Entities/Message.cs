namespace TeamChat.Domain.Entities;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ChatId { get; set; }
    public Guid SenderId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; } = null!;
    public User Sender { get; set; } = null!;
    public ICollection<MessageReadStatus> ReadStatuses { get; set; } = [];
    public ICollection<MessageAttachment> Attachments { get; set; } = [];
}
