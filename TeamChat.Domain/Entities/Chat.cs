namespace TeamChat.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Owner { get; set; } = null!;
    public ICollection<ChatMember> Members { get; set; } = [];
    public ICollection<ChatRole> Roles { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}
