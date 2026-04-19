using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Enums;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatRepository(AppDbContext context) 
    : BasicRepository<Chat, Guid>(context), IChatRepository
{
    public async Task<List<Chat>> GetUserChatsAsync(Guid userId)
    {
        return await _context.ChatMembers
            .Where(cm => cm.UserId == userId)
            .Select(cm => cm.Chat)
            .Include(c => c.Roles)
            .ToListAsync();
    }

    public override async Task<Chat?> GetByIdAsync(Guid id)
    {
        return await _context.Chats
            .Include(c => c.Members)
            .ThenInclude(m => m.Roles)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Chat>> GetUserCompanyChatsAsync(Guid userId, int companyId)
    {
        return await _context.Chats
            .Where(c => c.CompanyId == companyId &&
                        c.Members.Any(m => m.UserId == userId))
            .ToListAsync();
    }

    public async Task<List<Chat>> GetByDepartment(int? departmentId)
    {
        return await _context.Chats
            .Where(c => c.DepartmentId == departmentId)
            .ToListAsync();
    }
    public async Task<Chat?> GetPrivateChatAsync(Guid userId1, Guid userId2, int companyId)
    => await _context.Chats
        .Include(c => c.Members)
        .FirstOrDefaultAsync(c =>
            c.CompanyId == companyId &&
            c.Scope == ChatScope.Private &&
            c.Members.Count == 2 &&
            c.Members.Any(m => m.UserId == userId1) &&
            c.Members.Any(m => m.UserId == userId2));
}