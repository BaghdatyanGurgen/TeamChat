namespace TeamChat.Domain.Entities;

public class Message
{
    // Primary Key
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Properties
    public string Content { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
    
    public Guid SenderId { get; set; }
    public User Sender { get; set; } = null!;
    
    // Navigation Properties
    public ICollection<MessageReadStatus> ReadStatuses { get; set; } = [];
    public ICollection<MessageAttachment> Attachments { get; set; } = [];
}