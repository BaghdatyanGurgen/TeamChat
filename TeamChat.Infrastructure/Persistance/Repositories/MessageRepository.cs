using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class MessageRepository(AppDbContext context) 
    : BasicRepository<Message, Guid>(context), IMessageRepository
{
    public override async Task<Message?> GetByIdAsync(Guid id)
    {
        return await _context.Messages
                             .Include(m => m.ReadStatuses)
                             .Include(m => m.Attachments)
                             .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Message>> GetMessagesForChatAsync(Guid chatId)
    {
        return await _context.Messages
                             .Where(m => m.ChatId == chatId)
                             .Include(m => m.Attachments)
                             .ToListAsync();
    }
}