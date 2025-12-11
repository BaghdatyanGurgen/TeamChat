namespace TeamChat.Domain.Entities;

public class Team
{
    // Primary Key
    public int Id { get; set; }
    
    // Properties
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;
    
    // Navigation Properties
    public ICollection<TeamMember> Members { get; set; } = [];
    public ICollection<Chat> Chats { get; set; } = [];
}