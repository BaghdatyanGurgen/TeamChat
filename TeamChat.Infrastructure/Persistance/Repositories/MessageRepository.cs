using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class MessageRepository(AppDbContext context) : IMessageRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Message?> GetByIdAsync(Guid id) =>
        await _context.Messages.Include(m => m.ReadStatuses).Include(m => m.Attachments).FirstOrDefaultAsync(m => m.Id == id);

    public async Task AddAsync(Message message)
    {
        await _context.Messages.AddAsync(message);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Message>> GetChatMessagesAsync(Guid chatId) =>
        await _context.Messages.Where(m => m.ChatId == chatId).Include(m => m.Attachments).ToListAsync();

    public async Task UpdateAsync(Message message)
    {
        _context.Messages.Update(message);
        await _context.SaveChangesAsync();
    }
}