using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Message;

namespace TeamChat.Application.Abstraction.Services;

public interface IMessageService
{
    Task<ResponseModel<MessageResponse>> CreateMessageAsync(Guid userId, CreateMessageRequest request);
    Task<ResponseModel<IEnumerable<MessageResponse>>> GetChatMessagesAsync(Guid userId, Guid chatId);
    Task<ResponseModel<MessageResponse>> EditMessageAsync(Guid userId, Guid messageId, string newContent);
    Task<ResponseModel<MessageResponse>> DeleteMessageAsync(Guid userId, Guid messageId);
    Task<ResponseModel> MarkMessageAsReadAsync(Guid chatId, Guid messageId, Guid userId);
    Task<ResponseModel<IEnumerable<MessageResponse>>> GetMessagesPagedAsync(Guid chatId, int page, int pageSize);
}