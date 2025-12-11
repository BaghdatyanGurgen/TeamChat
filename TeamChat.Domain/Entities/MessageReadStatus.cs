namespace TeamChat.Domain.Entities;

public class MessageReadStatus
{
    // Primary Key
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Properties
    public DateTime ReadAt { get; set; } = DateTime.UtcNow;
    
    // Foreign Keys
    public Guid MessageId { get; set; }
    public Message Message { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}