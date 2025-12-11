namespace TeamChat.Domain.Entities;

public class ChatMember
{
    // Primary Key
    public Guid Id { get; set; } = Guid.NewGuid();

    // Properties
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
    
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    // Navigation Properties
    public ICollection<ChatMemberRole> Roles { get; set; } = [];
}