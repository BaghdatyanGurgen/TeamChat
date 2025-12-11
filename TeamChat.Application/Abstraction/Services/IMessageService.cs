using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Message;

namespace TeamChat.Application.Abstraction.Services;

public interface IMessageService
{
    Task<ResponseModel<MessageResponse>> CreateMessageAsync(Guid userId, CreateMessageRequest request);
    Task<ResponseModel<IEnumerable<MessageResponse>>> GetChatMessagesAsync(Guid userId, Guid chatId);
}