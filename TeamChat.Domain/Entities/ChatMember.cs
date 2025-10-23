namespace TeamChat.Domain.Entities;

public class ChatMember
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ChatId { get; set; }
    public Guid UserId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Chat Chat { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<ChatMemberRole> Roles { get; set; } = [];
}
