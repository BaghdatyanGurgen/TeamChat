using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatRoleRepository : BasicRepository<ChatRole, Guid>, IChatRoleRepository
{
    public ChatRoleRepository(AppDbContext context) : base(context) { }

    public async Task<List<ChatRole>> GetByChatIdAsync(Guid chatId)
    {
        return await _context.ChatRoles
                             .Where(r => r.ChatId == chatId)
                             .ToListAsync();
    }
}
