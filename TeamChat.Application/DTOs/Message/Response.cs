namespace TeamChat.Application.DTOs.Message;

public record MessageResponse(Guid Id, Guid UserId, string Content, DateTime CreatedAt)
{
    public MessageResponse(Domain.Entities.Message m)
        : this(m.Id, m.SenderId, m.Content, m.SentAt) { }
}