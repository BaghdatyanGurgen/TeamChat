namespace TeamChat.Domain.Entities;

public class TeamMember
{
    public int Id { get; set; }
    public int TeamId { get; set; }
    public int CompanyUserId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Team Team { get; set; } = null!;
    public CompanyUser CompanyUser { get; set; } = null!;
}