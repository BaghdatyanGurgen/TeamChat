using TeamChat.Domain.Entities;

namespace TeamChat.Domain.Interfaces
{
    public interface IChatMemberRoleRepository
    {
        Task<List<ChatMemberRole>> GetRolesByMemberIdAsync(Guid chatMemberId);
        Task AddAsync(ChatMemberRole memberRole);
        Task RemoveAsync(ChatMemberRole memberRole);
        Task<bool> ExistsAsync(Guid chatMemberId, Guid chatRoleId);
    }
}
