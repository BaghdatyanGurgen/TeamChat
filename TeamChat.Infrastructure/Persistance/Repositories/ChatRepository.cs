using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories
{
    public class ChatRepository : BasicRepository<Chat, Guid>, IChatRepository
    {
        public ChatRepository(AppDbContext context) : base(context) { }

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
    }
}