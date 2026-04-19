using TeamChat.Domain.Enums;

namespace TeamChat.Domain.Entities;

public class Chat
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Topic { get; set; } = string.Empty;
    public Guid? PinnedMessageId { get; set; }
    public ChatScope Scope { get; set; } = ChatScope.Company;
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }
    public int? TeamId { get; set; }
    public Team? Team { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    public ICollection<ChatMember> Members { get; set; } = [];
    public ICollection<ChatRole> Roles { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
    public ICollection<ChatPositionAccess> PositionAccess { get; set; } = [];
}