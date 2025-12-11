namespace TeamChat.Domain.Entities;

public class UserRefreshToken
{
    // Primary Key
    public Guid Id { get; set; }

    // Properties
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }
    public string PlainToken { get; set; } = string.Empty;

    // Foreign Keys
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}