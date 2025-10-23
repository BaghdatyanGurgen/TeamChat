using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Interfaces;
using TeamChat.Infrastructure.Persistence;

namespace TeamChat.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly AppDbContext _db;

        public ChatRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Chat?> GetByIdAsync(Guid id) =>
            await _db.Chats.Include(c => c.Members).ThenInclude(cm => cm.Roles).FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Chat chat)
        {
            await _db.Chats.AddAsync(chat);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Chat chat)
        {
            _db.Chats.Update(chat);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Chat>> GetUserChatsAsync(Guid userId) =>
            await _db.ChatMembers
                     .Where(cm => cm.UserId == userId)
                     .Select(cm => cm.Chat)
                     .Include(c => c.Roles)
                     .ToListAsync();
    }
}
