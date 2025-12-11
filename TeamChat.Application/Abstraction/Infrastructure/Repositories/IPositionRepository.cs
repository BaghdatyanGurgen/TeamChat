using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IPositionRepository : IBasicRepository<Position, int>
{
    Task<bool> CanCreateChat(Guid ownerId, int companyId);
}