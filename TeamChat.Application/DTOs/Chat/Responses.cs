namespace TeamChat.Application.DTOs.Chat;
public record ChatResponse(Guid Id, string Name, Guid OwnerId, DateTime CreatedAt)
{
    public ChatResponse(Domain.Entities.Chat chat) : this(chat.Id, chat.Name, chat.OwnerId, chat.CreatedAt) { }
}
public record ChatMemberResponse(Guid Id, Guid ChatId, Guid UserId, DateTime JoinedAt);