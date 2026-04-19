using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatPositionAccessRepository
{
    Task<ChatPositionAccess?> GetByPositionAndChatAsync(int positionId, Guid chatId);
    Task<List<Chat>> GetChatsByPositionAsync(int positionId);
    Task<List<ChatPositionAccess>> GetByChatAsync(Guid chatId);
    Task<ChatPositionAccess> AddAsync(ChatPositionAccess access);
    Task RemoveAsync(ChatPositionAccess access);
}