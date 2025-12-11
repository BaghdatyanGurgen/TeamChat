using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Infrastructure.Persistance.Repositories.Base;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatMemberRepository(AppDbContext db) 
    : BasicRepository<ChatMember, Guid>(db), IChatMemberRepository
{
    public async Task<ChatMember?> GetByUserAndChatAsync(Guid userId, Guid chatId)
    {
        return await _context.ChatMembers
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ChatId == chatId);
    }
}