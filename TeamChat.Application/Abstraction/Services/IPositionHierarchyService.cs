using TeamChat.Domain.Enums;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Services;

public interface IPositionHierarchyService
{
    Task<List<Position>> GetDescendantPositionsAsync(int positionId);
    Task<List<Position>> GetAncestorPositionsAsync(int positionId);
    Task<PositionPermissions> GetEffectivePermissionsAsync(int positionId, Guid chatId);
    Task<List<Chat>> GetAccessibleChatsAsync(int positionId);
}