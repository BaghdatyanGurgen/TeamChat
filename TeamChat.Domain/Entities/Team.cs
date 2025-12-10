namespace TeamChat.Domain.Entities;

public class Team
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Company Company { get; set; } = null!;
    public ICollection<TeamMember> Members { get; set; } = [];
    public ICollection<Chat> Chats { get; set; } = [];
}
