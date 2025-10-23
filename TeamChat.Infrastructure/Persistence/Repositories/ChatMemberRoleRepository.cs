using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Interfaces;

namespace TeamChat.Infrastructure.Persistence.Repositories
{
    public class ChatMemberRoleRepository : IChatMemberRoleRepository
    {
        private readonly AppDbContext _db;

        public ChatMemberRoleRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<List<ChatMemberRole>> GetRolesByMemberIdAsync(Guid chatMemberId) =>
            await _db.ChatMemberRoles
                     .Where(r => r.ChatMemberId == chatMemberId)
                     .Include(r => r.ChatRole)
                     .ToListAsync();

        public async Task AddAsync(ChatMemberRole memberRole)
        {
            if (!await ExistsAsync(memberRole.ChatMemberId, memberRole.ChatRoleId))
            {
                await _db.ChatMemberRoles.AddAsync(memberRole);
                await _db.SaveChangesAsync();
            }
        }

        public async Task RemoveAsync(ChatMemberRole memberRole)
        {
            _db.ChatMemberRoles.Remove(memberRole);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(Guid chatMemberId, Guid chatRoleId) =>
            await _db.ChatMemberRoles.AnyAsync(r => r.ChatMemberId == chatMemberId && r.ChatRoleId == chatRoleId);
    }
}
