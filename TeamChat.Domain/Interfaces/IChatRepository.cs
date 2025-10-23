using TeamChat.Domain.Entities;

namespace TeamChat.Domain.Interfaces
{
    public interface IChatRepository
    {
        Task<Chat?> GetByIdAsync(Guid id);
        Task AddAsync(Chat chat);
        Task UpdateAsync(Chat chat);
        Task<List<Chat>> GetUserChatsAsync(Guid userId);
    }
}
