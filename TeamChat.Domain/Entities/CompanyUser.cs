namespace TeamChat.Domain.Entities;

public class CompanyUser
{
    // Primary Key
    public int Id { get; set; }

    // Properties
    public bool IsActive { get; set; } = true;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign Keys
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;
}