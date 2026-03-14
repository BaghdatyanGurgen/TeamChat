using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Application.DTOs.Company;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IPositionRepository : IBasicRepository<Position, int>
{
    Task<bool> CanCreateChat(Guid ownerId, int companyId);
    Task<Position?> GetByInviteCodeAsync(string inviteCode);
    Task<List<Position>> GetUserPositionsAsync(Guid userId, int companyId);

}