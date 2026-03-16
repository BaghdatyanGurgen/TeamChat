using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;

namespace TeamChat.Application.Abstraction.Services;

public interface IChatService
{
    Task<ResponseModel<CreateChatResponse>> CreateChatAsync(Guid userId, CreateChatRequest request);
    Task EditMessageAsync(Guid messageId, string newContent, Guid editedBy);
    Task DeleteMessageAsync(Guid messageId, Guid deletedBy);
    Task MarkMessageAsReadAsync(Guid chatId, Guid messageId, Guid userId);
    Task<ResponseModel<IEnumerable<ChatResponse>>> GetUserChatsAsync(Guid userId);
    Task<ResponseModel> AddUserToChatAsync(Guid chatId, Guid addedBy, Guid newUserId);
    Task<ResponseModel> RemoveUserFromChatAsync(Guid chatId, Guid removedBy, Guid userId);
    Task<ResponseModel> AssignChatAdminAsync(Guid chatId, Guid userId);
    Task<ResponseModel> DeleteChatAsync(Guid chatId, Guid requestedBy);
    Task<ResponseModel<List<CompanyChatResponse>>> GetUserCompanyChatsAsync(Guid userId, int companyId);
}