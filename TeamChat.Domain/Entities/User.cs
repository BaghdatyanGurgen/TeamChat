namespace TeamChat.Domain.Entities;

public class User
{
    // Primary Key
    public Guid Id { get; set; } = Guid.NewGuid();
 
    // Properties
    public string Email { get; set; } = string.Empty;
    public bool EmailConfirmed { get; set; } = false;
    public string? EmailConfirmationCode { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? AvatarUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public ICollection<ChatMember> ChatMemberships { get; set; } = [];
    public ICollection<CompanyUser> CompanyMemberships { get; set; } = [];
    public ICollection<Company> ManagedCompanies { get; set; } = [];
}