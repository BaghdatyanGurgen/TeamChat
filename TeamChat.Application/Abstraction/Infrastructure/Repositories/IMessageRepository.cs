using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IMessageRepository : IBasicRepository<Message, Guid>
{
    Task<List<Message>> GetMessagesForChatAsync(Guid chatId);
}