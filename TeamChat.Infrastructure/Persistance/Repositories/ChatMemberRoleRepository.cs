using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatMemberRoleRepository : BasicRepository<ChatMemberRole, Guid>, IChatMemberRoleRepository
{
    public ChatMemberRoleRepository(AppDbContext context) : base(context) { }

    public async Task<List<ChatMemberRole>> GetRolesByMemberIdAsync(Guid chatMemberId) =>
        await _context.ChatMemberRoles
                      .Where(r => r.ChatMemberId == chatMemberId)
                      .Include(r => r.ChatRole)
                      .ToListAsync();

    public async Task<bool> ExistsAsync(Guid chatMemberId, Guid chatRoleId) =>
        await _context.ChatMemberRoles.AnyAsync(r => r.ChatMemberId == chatMemberId && r.ChatRoleId == chatRoleId);
}
