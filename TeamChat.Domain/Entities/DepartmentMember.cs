namespace TeamChat.Domain.Entities;

public class DepartmentMember
{
    // Primary Key
    public int Id { get; set; }
    
    // Properties
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    // Foreign Keys
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
    
    public int CompanyUserId { get; set; }
    public CompanyUser CompanyUser { get; set; } = null!;
}