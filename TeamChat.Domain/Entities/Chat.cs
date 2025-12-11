namespace TeamChat.Domain.Entities;

public class Chat
{
    //Primary key
    public Guid Id { get; set; } = Guid.NewGuid();

    // Properties
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign keys
    public Guid OwnerId { get; set; }
    public User Owner { get; set; } = null!;
    
    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    public int? TeamId { get; set; }
    public Team? Team { get; set; }

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    // Navigation properties
    public ICollection<ChatMember> Members { get; set; } = [];
    public ICollection<ChatRole> Roles { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];
}