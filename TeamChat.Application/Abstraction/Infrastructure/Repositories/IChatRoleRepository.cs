using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatRoleRepository : IBasicRepository<ChatRole, Guid>
{
    Task<List<ChatRole>> GetByChatIdAsync(Guid chatId);
}