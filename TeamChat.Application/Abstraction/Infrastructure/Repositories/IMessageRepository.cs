using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IMessageRepository
{
    Task<Message?> GetByIdAsync(Guid id);
    Task AddAsync(Message message);
    Task<List<Message>> GetChatMessagesAsync(Guid chatId);
    Task UpdateAsync(Message message);
}