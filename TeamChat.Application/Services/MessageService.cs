using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Message;
using TeamChat.Domain.Models.Exceptions;
using TeamChat.Messaging.Contracts.Message;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.File;
using TeamChat.Application.Abstraction.Infrastructure.Messaging;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;

public class MessageService(
    IMessageRepository messageRepository,
    IChatRepository chatRepository,
    IChatMemberRepository chatMemberRepository,
    IUserRepository userRepository,
    IMessagePublisher messagePublisher,
    IFileService fileService) : IMessageService
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IChatMemberRepository _chatMemberRepository = chatMemberRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;
    private readonly IFileService _fileService = fileService;

    public async Task<ResponseModel<MessageResponse>> CreateMessageAsync(Guid userId, CreateMessageRequest request)
    {
        var chat = await _chatRepository.GetByIdAsync(request.ChatId) ?? throw new ChatNotFoundException();
        _ = await _chatMemberRepository.GetByUserAndChatAsync(userId, request.ChatId) ?? throw new NoAccessException();
        var user = await _userRepository.GetByIdAsync(userId) ?? throw new UserNotFoundException();

        var message = new Message
        {
            ChatId = chat.Id,
            SenderId = userId,
            Sender = user,
            Content = request.Content,
            SentAt = DateTime.UtcNow
        };

        var created = await _messageRepository.AddAsync(message);

        if (request.Attachment != null)
        {
            var (relativeUrl, originalFileName) = await _fileService.UploadFileAsync(request.Attachment, $"messages/{chat.Id}");
            var parts = relativeUrl.TrimStart('/').Split('/', 3);
            var folder = parts.Length >= 3 ? parts[1] : $"messages/{chat.Id}";
            var fileName = parts.Length >= 3 ? parts[2] : relativeUrl;

            await _messageRepository.AddAttachmentAsync(new MessageAttachment
            {
                MessageId = created.Id,
                FileUrl = $"/api/files/{folder}/{fileName}",
                OriginalFileName = originalFileName
            });
        }

        await _messagePublisher.PublishAsync(new MessageCreatedEvent(
            new MessageCreatedPayload(chat.Id, created.Id, created.SenderId, created.Content, created.SentAt)));

        var withAttachments = await _messageRepository.GetByIdAsync(created.Id) ?? created;
        return ResponseModel<MessageResponse>.Success(new MessageResponse(withAttachments));
    }

    public async Task<ResponseModel<IEnumerable<MessageResponse>>> GetChatMessagesAsync(Guid userId, Guid chatId)
    {
        _ = await _chatRepository.GetByIdAsync(chatId) ?? throw new ChatNotFoundException();
        _ = await _chatMemberRepository.GetByUserAndChatAsync(userId, chatId) ?? throw new NoAccessException();

        var messages = await _messageRepository.GetMessagesForChatAsync(chatId);
        return ResponseModel<IEnumerable<MessageResponse>>.Success(messages.Select(m => new MessageResponse(m)));
    }

    public async Task<ResponseModel<MessageResponse>> EditMessageAsync(Guid userId, Guid messageId, string newContent)
    {
        var message = await _messageRepository.GetByIdAsync(messageId) ?? throw new MessageNotFoundException();
        if (message.SenderId != userId) throw new NoAccessException();

        message.Content = newContent;
        message.EditedAt = DateTime.UtcNow;
        await _messageRepository.UpdateAsync(message);

        return ResponseModel<MessageResponse>.Success(new MessageResponse(message));
    }

    public async Task<ResponseModel<MessageResponse>> DeleteMessageAsync(Guid userId, Guid messageId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId) ?? throw new MessageNotFoundException();
        if (message.SenderId != userId) throw new NoAccessException();

        await _messageRepository.RemoveAsync(message);
        return ResponseModel<MessageResponse>.Success(new MessageResponse(message));
    }

    public async Task<ResponseModel> MarkAllAsReadAsync(Guid chatId, Guid userId)
    {
        _ = await _chatRepository.GetByIdAsync(chatId) ?? throw new ChatNotFoundException();
        _ = await _chatMemberRepository.GetByUserAndChatAsync(userId, chatId) ?? throw new NoAccessException();

        await _messageRepository.MarkAllAsReadAsync(chatId, userId);
        return ResponseModel.Success("Messages marked as read.");
    }

    public async Task<ResponseModel<Dictionary<Guid, int>>> GetUnreadCountsAsync(Guid userId, int companyId)
    {
        var memberships = await _chatMemberRepository.GetChatsByUserIdAsync(userId);
        var counts = new Dictionary<Guid, int>();

        foreach (var membership in memberships)
        {
            var count = await _messageRepository.GetUnreadCountAsync(membership.ChatId, userId);
            if (count > 0) counts[membership.ChatId] = count;
        }

        return ResponseModel<Dictionary<Guid, int>>.Success(counts);
    }

    public async Task<ResponseModel<MessageResponse>> TagMessageAsync(Guid userId, Guid messageId, string tag)
    {
        var message = await _messageRepository.GetByIdAsync(messageId) ?? throw new MessageNotFoundException();
        _ = await _chatMemberRepository.GetByUserAndChatAsync(userId, message.ChatId) ?? throw new NoAccessException();

        message.Tag = tag;
        await _messageRepository.UpdateAsync(message);
        return ResponseModel<MessageResponse>.Success(new MessageResponse(message));
    }

    public Task<ResponseModel<IEnumerable<MessageResponse>>> GetMessagesPagedAsync(Guid chatId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}