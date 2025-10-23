using TeamChat.Domain.Entities;

namespace TeamChat.Domain.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message?> GetByIdAsync(Guid id);
        Task AddAsync(Message message);
        Task<List<Message>> GetChatMessagesAsync(Guid chatId);
        Task UpdateAsync(Message message);
    }
}
