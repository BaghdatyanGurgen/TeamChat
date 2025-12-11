namespace TeamChat.Domain.Entities;

public class MessageAttachment
{
    // Primary Key
    public Guid Id { get; set; } = Guid.NewGuid();
    
    // Properties
    public string FileUrl { get; set; } = string.Empty;
    
    // Foreign Keys
    public Guid MessageId { get; set; }
    public Message Message { get; set; } = null!;
}