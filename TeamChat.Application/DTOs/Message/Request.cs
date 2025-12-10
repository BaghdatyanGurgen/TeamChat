namespace TeamChat.Application.DTOs.Message;

public record CreateMessageRequest(Guid ChatId, string Content);