using TeamChat.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChat.Domain.Entities;

public class Position
{
    public int Id { get; set; }

    public Guid CreatedByUserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string InviteCode { get; set; } = string.Empty;

    [Column(TypeName = "integer")]
    public PositionPermissions Permissions { get; set; } = PositionPermissions.None;

    // Foreign Keys
    public int CompanyId { get; set; }
    public Company Company { get; set; } = null!;

    public int? ParentPositionId { get; set; }
    public Position? ParentPosition { get; set; }

    public int? DepartmentId { get; set; }
    public Department? Department { get; set; }

    // Navigation
    public ICollection<Position> SubPositions { get; set; } = [];
    public ICollection<CompanyUser> AssignedUsers { get; set; } = [];
    public ICollection<ChatPositionAccess> ChatAccess { get; set; } = [];
}