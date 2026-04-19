using TeamChat.Domain.Entities;
using TeamChat.Application.Abstraction.Infrastructure.Repositories.Base;

namespace TeamChat.Application.Abstraction.Infrastructure.Repositories;

public interface IMessageRepository : IBasicRepository<Message, Guid>
{
    Task<IEnumerable<Message>> GetMessagesByTagAsync(Guid chatId, string tag);
    Task<List<Message>> GetMessagesForChatAsync(Guid chatId);
    Task AddReadStatusAsync(MessageReadStatus messageReadStatus);
    Task<MessageReadStatus?> GetReadStatusAsync(Guid messageId, Guid userId);
    Task UpdateReadStatusAsync(object existingRead);
    Task<int> GetUnreadCountAsync(Guid chatId, Guid userId);
    Task MarkAllAsReadAsync(Guid chatId, Guid userId);
    Task AddAttachmentAsync(MessageAttachment attachment);
}