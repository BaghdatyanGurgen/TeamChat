using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatMemberRepository : IBasicRepository<ChatMember, Guid>
{
    Task<ChatMember?> GetByUserAndChatAsync(Guid userId, Guid chatId);
}