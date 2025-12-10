using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;

namespace TeamChat.Application.Abstraction.Services;

public interface IChatService
{
    Task<ResponseModel<ChatResponse>> CreateChatAsync(Guid userId, CreateChatRequest request);
}
