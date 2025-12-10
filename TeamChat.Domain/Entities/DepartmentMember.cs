namespace TeamChat.Domain.Entities;

public class DepartmentMember
{
    public int Id { get; set; }
    public int DepartmentId { get; set; }
    public int CompanyUserId { get; set; }
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

    public Department Department { get; set; } = null!;
    public CompanyUser CompanyUser { get; set; } = null!;
}