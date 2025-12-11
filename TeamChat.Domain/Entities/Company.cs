namespace TeamChat.Domain.Entities;

public class Company
{
    // Primary Key
    public int Id { get; set; }

    // Properties
    public string Name { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }

    // Foreign Keys
    public Guid DirectorId { get; set; }
    public User Director { get; set; } = null!;

    // Navigation Properties
    public ICollection<Chat> Chats { get; set; } = [];    
    public ICollection<Position> Positions { get; set; } = [];
    public ICollection<CompanyUser> Members { get; set; } = [];
}