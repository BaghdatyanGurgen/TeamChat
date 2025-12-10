using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Domain.Entities;
using TeamChat.Infrastructure.Persistance.Repositories.Base;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatMemberRepository : BasicRepository<ChatMember, Guid>, IChatMemberRepository
{
    public ChatMemberRepository(AppDbContext db) : base(db) { }
    public async Task<ChatMember?> GetByUserAndChatAsync(Guid userId, Guid chatId)
    {
        return await _context.ChatMembers
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ChatId == chatId);
    }

}