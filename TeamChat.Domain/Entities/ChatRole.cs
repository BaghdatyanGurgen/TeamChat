namespace TeamChat.Domain.Entities;

public class ChatRole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ChatId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public Chat Chat { get; set; } = null!;
    public ICollection<ChatMemberRole> MemberRoles { get; set; } = [];
}