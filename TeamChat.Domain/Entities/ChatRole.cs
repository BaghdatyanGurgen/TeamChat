namespace TeamChat.Domain.Entities;

public class ChatRole
{
    // Primary Key
    public Guid Id { get; set; } = Guid.NewGuid();

    // Properties
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    // Foreign Keys
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = null!;

    // Navigation Properties
    public ICollection<ChatMemberRole> MemberRoles { get; set; } = [];
}