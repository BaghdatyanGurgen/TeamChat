namespace TeamChat.Application.DTOs.Message;

public record MessageResponse(Guid Id, Guid ChatId, Guid SenderId, string SenderName, string? SenderAvatarUrl, string Content, DateTime CreatedAt, DateTime? EditedAt)
{
    public MessageResponse(Domain.Entities.Message m)
        : this(m.Id, m.ChatId, m.SenderId, $"{m.Sender.FirstName} {m.Sender.LastName}".Trim(), m.Sender.AvatarUrl, m.Content, m.SentAt, m.EditedAt == default ? null : m.EditedAt) { }
}

public record EditMessageRequest(string Content);