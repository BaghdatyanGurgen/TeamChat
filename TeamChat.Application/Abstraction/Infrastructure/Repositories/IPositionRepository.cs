using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IPositionRepository : IBasicRepository<Position, int>
{
    Task<bool> CanCreateChat(Guid ownerId, int companyId);
    Task<Position?> GetByInviteCodeAsync(string inviteCode);
    Task<List<Position>> GetUserPositionsAsync(Guid userId, int companyId); 
    Task<List<Position>> GetChildrenAsync(int positionId);
}