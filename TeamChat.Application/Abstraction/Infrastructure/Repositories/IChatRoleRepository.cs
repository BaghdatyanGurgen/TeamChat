using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatRoleRepository
{
    Task<ChatRole?> GetByIdAsync(Guid id);
    Task<List<ChatRole>> GetByChatIdAsync(Guid chatId);
    Task AddAsync(ChatRole role);
    Task UpdateAsync(ChatRole role);
    Task DeleteAsync(ChatRole role);
}