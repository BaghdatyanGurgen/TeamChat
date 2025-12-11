using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatMemberRoleRepository(AppDbContext context)
        : BasicRepository<ChatMemberRole, Guid>(context), IChatMemberRoleRepository
{
    public async Task<List<ChatMemberRole>> GetRolesByMemberIdAsync(Guid chatMemberId)
    {
        var result = await _context.ChatMemberRoles
                                   .Where(r => r.ChatMemberId == chatMemberId)
                                   .Include(r => r.ChatRole)
                                   .ToListAsync();
        return result;
    }
    public async Task<bool> ExistsAsync(Guid chatMemberId, Guid chatRoleId) =>
        await _context.ChatMemberRoles.AnyAsync(r => r.ChatMemberId == chatMemberId && r.ChatRoleId == chatRoleId);
}