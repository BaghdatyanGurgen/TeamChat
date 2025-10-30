using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories
{
    public class ChatRepository(AppDbContext context) : IChatRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Chat?> GetByIdAsync(Guid id) =>
            await _context.Chats.Include(c => c.Members).ThenInclude(cm => cm.Roles).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Chat chat)
        {
            await _context.Chats.AddAsync(chat);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Chat chat)
        {
            _context.Chats.Update(chat);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Chat>> GetUserChatsAsync(Guid userId) =>
            await _context.ChatMembers
                     .Where(cm => cm.UserId == userId)
                     .Select(cm => cm.Chat)
                     .Include(c => c.Roles)
                     .ToListAsync();
    }
}