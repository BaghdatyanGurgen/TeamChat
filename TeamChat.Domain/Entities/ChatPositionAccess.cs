using TeamChat.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamChat.Domain.Entities;

public class ChatPositionAccess
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;
    public Guid ChatId { get; set; }
    public Chat Chat { get; set; } = null!;
    [Column(TypeName = "integer")]
    public PositionPermissions? PermissionOverride { get; set; }
}