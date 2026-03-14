namespace TeamChat.Application.DTOs.Message;

public record MessageResponse(Guid Id, Guid SenderId, string SenderName, string? SenderAvatarUrl, string Content, DateTime CreatedAt)
{
    public MessageResponse(Domain.Entities.Message m)
        : this(m.Id, m.SenderId, $"{m.Sender.FirstName} {m.Sender.LastName}".Trim(), m.Sender.AvatarUrl, m.Content, m.SentAt) { }
}