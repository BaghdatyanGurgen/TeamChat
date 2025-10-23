using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Interfaces;

namespace TeamChat.Infrastructure.Persistence.Repositories
{
    public class ChatRoleRepository : IChatRoleRepository
    {
        private readonly AppDbContext _db;

        public ChatRoleRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ChatRole?> GetByIdAsync(Guid id) =>
            await _db.ChatRoles.FirstOrDefaultAsync(r => r.Id == id);

        public async Task<List<ChatRole>> GetByChatIdAsync(Guid chatId) =>
            await _db.ChatRoles.Where(r => r.ChatId == chatId).ToListAsync();

        public async Task AddAsync(ChatRole role)
        {
            await _db.ChatRoles.AddAsync(role);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(ChatRole role)
        {
            _db.ChatRoles.Update(role);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(ChatRole role)
        {
            _db.ChatRoles.Remove(role);
            await _db.SaveChangesAsync();
        }
    }
}
