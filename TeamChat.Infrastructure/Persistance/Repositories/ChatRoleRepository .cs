using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatRoleRepository(AppDbContext context)
        : BasicRepository<ChatRole, Guid>(context), IChatRoleRepository
{
    public async Task<List<ChatRole>> GetByChatIdAsync(Guid chatId)
    {
        return await _context.ChatRoles
                             .Where(r => r.ChatId == chatId)
                             .ToListAsync();
    }
}