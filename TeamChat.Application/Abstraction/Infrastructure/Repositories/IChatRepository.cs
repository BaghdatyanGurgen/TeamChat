using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IChatRepository
{
    Task<Chat?> GetByIdAsync(Guid id);
    Task AddAsync(Chat chat);
    Task UpdateAsync(Chat chat);
    Task<List<Chat>> GetUserChatsAsync(Guid userId);
}