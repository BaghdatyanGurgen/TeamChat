using TeamChat.Domain.Entities;
using TeamChat.Domain.Enums;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;

public class PositionHierarchyService(
    IPositionRepository positionRepository,
    IChatPositionAccessRepository chatPositionAccessRepository)
    : IPositionHierarchyService
{
    public async Task<List<Position>> GetDescendantPositionsAsync(int positionId)
    {
        var all = new List<Position>();
        var queue = new Queue<int>();
        queue.Enqueue(positionId);

        while (queue.Count > 0)
        {
            var currentId = queue.Dequeue();
            var children = await positionRepository
                .GetChildrenAsync(currentId);

            foreach (var child in children)
            {
                all.Add(child);
                queue.Enqueue(child.Id);
            }
        }

        return all;
    }

    public async Task<List<Position>> GetAncestorPositionsAsync(int positionId)
    {
        var ancestors = new List<Position>();
        var current = await positionRepository.GetByIdAsync(positionId);

        while (current?.ParentPositionId != null)
        {
            var parent = await positionRepository
                .GetByIdAsync(current.ParentPositionId.Value);
            if (parent == null) break;
            ancestors.Add(parent);
            current = parent;
        }

        return ancestors;
    }

    public async Task<PositionPermissions> GetEffectivePermissionsAsync(
        int positionId, Guid chatId)
    {
        var access = await chatPositionAccessRepository
            .GetByPositionAndChatAsync(positionId, chatId);

        if (access != null)
            return access.PermissionOverride
                ?? (await positionRepository.GetByIdAsync(positionId))!.Permissions;

        // Проверяем, есть ли доступ через дочерние позиции (наследование вверх)
        var position = await positionRepository.GetByIdAsync(positionId);
        return position?.Permissions ?? PositionPermissions.None;
    }

    public async Task<List<Chat>> GetAccessibleChatsAsync(int positionId)
    {
        var directAccess = await chatPositionAccessRepository
            .GetChatsByPositionAsync(positionId);

        var descendants = await GetDescendantPositionsAsync(positionId);
        var inheritedAccess = new List<Chat>();

        foreach (var descendant in descendants)
        {
            var chats = await chatPositionAccessRepository
                .GetChatsByPositionAsync(descendant.Id);
            inheritedAccess.AddRange(chats);
        }

        return directAccess
            .Concat(inheritedAccess)
            .DistinctBy(c => c.Id)
            .ToList();
    }
}