using Microsoft.AspNetCore.Http;

namespace TeamChat.Application.DTOs.Message;

public record MessageAttachmentResponse(Guid Id, string FileUrl, string OriginalFileName);

public record MessageResponse(
    Guid Id,
    Guid ChatId,
    Guid SenderId,
    string SenderName,
    string? SenderAvatarUrl,
    string Content,
    DateTime CreatedAt,
    DateTime? EditedAt,
    List<MessageAttachmentResponse> Attachments)
{
    public MessageResponse(Domain.Entities.Message m)
        : this(
            m.Id,
            m.ChatId,
            m.SenderId,
            $"{m.Sender.FirstName} {m.Sender.LastName}".Trim(),
            m.Sender.AvatarUrl,
            m.Content,
            m.SentAt,
            m.EditedAt == default ? null : m.EditedAt,
            m.Attachments?.Select(a => new MessageAttachmentResponse(a.Id, a.FileUrl, a.OriginalFileName)).ToList() ?? [])
    { }
}

public record EditMessageRequest(string Content);