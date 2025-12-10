using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatRepository: IBasicRepository<Chat, Guid>
{
    Task<List<Chat>> GetUserChatsAsync(Guid userId);
}