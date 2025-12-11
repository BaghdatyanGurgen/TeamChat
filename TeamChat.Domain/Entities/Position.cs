using TeamChat.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChat.Domain.Entities;

public class Position
{
    // Primary Key
    public int Id { get; set; }
    
    // Properties
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

    // Navigation Properties
    public ICollection<Position> SubPositions { get; set; } = [];
    public ICollection<CompanyUser> AssignedUsers { get; set; } = [];
}