using TeamChat.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Infrastructure.Persistance.Repositories;

public class ChatRoleRepository(AppDbContext context) : IChatRoleRepository
{
    private readonly AppDbContext _context = context;

    public async Task<ChatRole?> GetByIdAsync(Guid id) =>
        await _context.ChatRoles.FirstOrDefaultAsync(r => r.Id == id);

    public async Task<List<ChatRole>> GetByChatIdAsync(Guid chatId) =>
        await _context.ChatRoles.Where(r => r.ChatId == chatId).ToListAsync();

    public async Task AddAsync(ChatRole role)
    {
        await _context.ChatRoles.AddAsync(role);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ChatRole role)
    {
        _context.ChatRoles.Update(role);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ChatRole role)
    {
        _context.ChatRoles.Remove(role);
        await _context.SaveChangesAsync();
    }
}