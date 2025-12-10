namespace TeamChat.Domain.Entities;

public class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public Guid DirectorId { get; set; }
    public User Director { get; set; } = null!;

    public ICollection<Chat> Chats { get; set; } = [];
    
    public ICollection<Position> Positions { get; set; } = [];

    public ICollection<CompanyUser> Members { get; set; } = [];
    
    public string Description { get; set; } = string.Empty;

    public string? LogoUrl { get; set; }
}