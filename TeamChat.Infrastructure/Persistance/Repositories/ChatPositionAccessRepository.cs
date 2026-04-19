using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatPositionAccessRepository(AppDbContext context)
    : IChatPositionAccessRepository
{
    public async Task<ChatPositionAccess?> GetByPositionAndChatAsync(
        int positionId, Guid chatId)
        => await context.ChatPositionAccess
            .Include(x => x.Position)
            .Include(x => x.Chat)
            .FirstOrDefaultAsync(x =>
                x.PositionId == positionId && x.ChatId == chatId);

    public async Task<List<Chat>> GetChatsByPositionAsync(int positionId)
        => await context.ChatPositionAccess
            .Where(x => x.PositionId == positionId)
            .Select(x => x.Chat)
            .ToListAsync();

    public async Task<List<ChatPositionAccess>> GetByChatAsync(Guid chatId)
        => await context.ChatPositionAccess
            .Where(x => x.ChatId == chatId)
            .Include(x => x.Position)
            .ToListAsync();

    public async Task<ChatPositionAccess> AddAsync(ChatPositionAccess access)
    {
        context.ChatPositionAccess.Add(access);
        await context.SaveChangesAsync();
        return access;
    }

    public async Task RemoveAsync(ChatPositionAccess access)
    {
        context.ChatPositionAccess.Remove(access);
        await context.SaveChangesAsync();
    }
}