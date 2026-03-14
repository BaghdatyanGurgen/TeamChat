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

    public async Task<IEnumerable<Message>> GetMessagesByTagAsync(Guid chatId, string tag)
    {
        return await _context.Messages
                             .Where(m => m.ChatId == chatId && 
                                         m.Tag == tag)
                             .Include(m => m.Attachments)
                             .ToListAsync();
    }

    public async Task<List<Message>> GetMessagesForChatAsync(Guid chatId)
    {
        return await _context.Messages
                             .Where(m => m.ChatId == chatId)
                             .Include(m => m.Attachments)
                             .Include(m => m.Sender)
                             .ToListAsync();
    }

    public async Task<MessageReadStatus?> GetReadStatusAsync(Guid messageId, Guid userId)
    {
        return await _context.MessageReadStatuses
            .FirstOrDefaultAsync(mr => mr.MessageId == messageId && mr.UserId == userId);
    }

    public async Task UpdateReadStatusAsync(MessageReadStatus existingRead)
    {
        _context.MessageReadStatuses.Update(existingRead);
        await _context.SaveChangesAsync();
    }

    public async Task AddReadStatusAsync(MessageReadStatus messageReadStatus)
    {
        await _context.MessageReadStatuses.AddAsync(messageReadStatus);
        await _context.SaveChangesAsync();
    }

    public Task UpdateReadStatusAsync(object existingRead)
    {
        throw new NotImplementedException();
    }
}