namespace TeamChat.Domain.Entities;

public class TeamMember
{
    // Primary Key
    public int Id { get; set; }

    // Properties
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int TeamId { get; set; }
    public Team Team { get; set; } = null!;

    public int CompanyUserId { get; set; }
    public CompanyUser CompanyUser { get; set; } = null!;
}