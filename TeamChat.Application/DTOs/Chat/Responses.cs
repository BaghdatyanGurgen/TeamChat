namespace TeamChat.Application.DTOs.Chat;

public record ChatResponse(Guid Id, string Name, Guid OwnerId, DateTime CreatedAt)
{
    public ChatResponse(Domain.Entities.Chat chat) : this(chat.Id, chat.Name, chat.OwnerId, chat.CreatedAt) { }
}

public record CreateChatResponse(Guid Id, string Name, Guid OwnerId, DateTime CreatedAt, List<Guid> ParticipantIds)
{
    public CreateChatResponse(Domain.Entities.Chat chat, List<Guid> participantIds)
        : this(chat.Id, chat.Name, chat.OwnerId, chat.CreatedAt, participantIds) { }
}

public record ChatMemberResponse(Guid Id, Guid ChatId, Guid UserId, DateTime JoinedAt);
public record CompanyChatResponse(Guid Id, string Name, DateTime CreatedAt);