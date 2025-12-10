using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatMemberRoleRepository : IBasicRepository<ChatMemberRole,Guid>
{
    Task<List<ChatMemberRole>> GetRolesByMemberIdAsync(Guid chatMemberId);
    Task<bool> ExistsAsync(Guid chatMemberId, Guid chatRoleId);
}