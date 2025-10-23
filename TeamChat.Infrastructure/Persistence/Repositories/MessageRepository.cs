using Microsoft.EntityFrameworkCore;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Interfaces;
using TeamChat.Infrastructure.Persistence;

namespace TeamChat.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly AppDbContext _db;

        public MessageRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Message?> GetByIdAsync(Guid id) =>
            await _db.Messages.Include(m => m.ReadStatuses).Include(m => m.Attachments).FirstOrDefaultAsync(m => m.Id == id);

        public async Task AddAsync(Message message)
        {
            await _db.Messages.AddAsync(message);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Message>> GetChatMessagesAsync(Guid chatId) =>
            await _db.Messages.Where(m => m.ChatId == chatId).Include(m => m.Attachments).ToListAsync();

        public async Task UpdateAsync(Message message)
        {
            _db.Messages.Update(message);
            await _db.SaveChangesAsync();
        }
    }
}
