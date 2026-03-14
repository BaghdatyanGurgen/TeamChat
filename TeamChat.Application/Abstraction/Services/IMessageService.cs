using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Message;

namespace TeamChat.Application.Abstraction.Services;

public interface IMessageService
{
    Task<ResponseModel<MessageResponse>> CreateMessageAsync(Guid userId, CreateMessageRequest request);
    Task<ResponseModel<IEnumerable<MessageResponse>>> GetChatMessagesAsync(Guid userId, Guid chatId);
    Task<ResponseModel<MessageResponse>> EditMessageAsync(Guid messageId, string newContent, Guid editedBy);
    Task<ResponseModel> DeleteMessageAsync(Guid messageId, Guid deletedBy); 
    Task<ResponseModel> MarkMessageAsReadAsync(Guid chatId, Guid messageId, Guid userId);
    Task<ResponseModel<IEnumerable<MessageResponse>>> GetMessagesPagedAsync(Guid chatId, int page, int pageSize);
}