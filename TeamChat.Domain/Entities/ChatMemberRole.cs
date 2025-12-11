namespace TeamChat.Domain.Entities;

public class ChatMemberRole
{
    // Primary Key
    public Guid Id { get; set; } = Guid.NewGuid();

    // Foreign Keys
    public Guid ChatMemberId { get; set; }
    public ChatMember ChatMember { get; set; } = null!;

    public Guid ChatRoleId { get; set; }
    public ChatRole ChatRole { get; set; } = null!;
}