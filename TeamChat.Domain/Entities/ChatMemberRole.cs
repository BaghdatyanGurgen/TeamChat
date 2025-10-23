namespace TeamChat.Domain.Entities;

public class ChatMemberRole
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid ChatMemberId { get; set; }
    public Guid ChatRoleId { get; set; }

    public ChatMember ChatMember { get; set; } = null!;
    public ChatRole ChatRole { get; set; } = null!;
}
