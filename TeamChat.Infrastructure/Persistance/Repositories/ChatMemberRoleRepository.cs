using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatMemberRoleRepository(AppDbContext context) : IChatMemberRoleRepository
{
    private readonly AppDbContext _context = context;

    public async Task<List<ChatMemberRole>> GetRolesByMemberIdAsync(Guid chatMemberId) =>
        await _context.ChatMemberRoles
                 .Where(r => r.ChatMemberId == chatMemberId)
                 .Include(r => r.ChatRole)
                 .ToListAsync();

    public async Task AddAsync(ChatMemberRole memberRole)
    {
        if (!await ExistsAsync(memberRole.ChatMemberId, memberRole.ChatRoleId))
        {
            await _context.ChatMemberRoles.AddAsync(memberRole);
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveAsync(ChatMemberRole memberRole)
    {
        _context.ChatMemberRoles.Remove(memberRole);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsAsync(Guid chatMemberId, Guid chatRoleId) =>
        await _context.ChatMemberRoles.AnyAsync(r => r.ChatMemberId == chatMemberId && r.ChatRoleId == chatRoleId);
}