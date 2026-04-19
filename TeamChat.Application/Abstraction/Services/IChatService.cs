using TeamChat.Domain.Enums;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;

namespace TeamChat.Application.Abstraction.Services;

public interface IChatService
{
    // Групповые чаты
    Task<ResponseModel<CreateChatResponse>> CreateChatAsync(Guid userId, CreateChatRequest request);

    // Чаты 1-на-1
    Task<ResponseModel<CreateChatResponse>> CreatePrivateChatAsync(Guid userId, Guid targetUserId, int companyId);

    // Привязка/отвязка позиций
    Task<ResponseModel> AttachPositionToChatAsync(Guid userId, Guid chatId, int positionId, PositionPermissions? permissionOverride = null);
    Task<ResponseModel> DetachPositionFromChatAsync(Guid userId, Guid chatId, int positionId);

    // Сообщения
    Task EditMessageAsync(Guid messageId, string newContent, Guid editedBy);
    Task DeleteMessageAsync(Guid messageId, Guid deletedBy);
    Task MarkMessageAsReadAsync(Guid chatId, Guid messageId, Guid userId);

    // Получение чатов
    Task<ResponseModel<List<CompanyChatResponse>>> GetUserCompanyChatsAsync(Guid userId, int companyId);

    // Управление
    Task<ResponseModel> DeleteChatAsync(Guid chatId, Guid requestedBy);
}