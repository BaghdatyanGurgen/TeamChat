using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatRoleRepository : IBasicRepository<ChatRole, Guid>
{
    Task<List<ChatRole>> GetByChatIdAsync(Guid chatId);
}