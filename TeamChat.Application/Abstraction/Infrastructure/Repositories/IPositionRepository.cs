using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IPositionRepository : IBasicRepository<Position, int>
{
    Task<bool> CanCreateChat(Guid ownerId, int companyId);
}