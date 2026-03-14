using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Message;
using TeamChat.Domain.Models.Exceptions;
using TeamChat.Messaging.Contracts.Message;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.Messaging;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;

public class MessageService(IMessageRepository messageRepository,
                            IChatRepository chatRepository,
                            IChatMemberRepository chatMemberRepository,
                            IUserRepository userRepository,
                            IMessagePublisher messagePublisher) : IMessageService
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IChatMemberRepository _chatMemberRepository = chatMemberRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;

    public async Task<ResponseModel<MessageResponse>> CreateMessageAsync(Guid userId, CreateMessageRequest request)
    {
        var chat = await _chatRepository.GetByIdAsync(request.ChatId)
            ?? throw new ChatNotFoundException();
        
        _ = await _chatMemberRepository.GetByUserAndChatAsync(userId, request.ChatId)
            ?? throw new NoAccessException();
        
        var user = await _userRepository.GetByIdAsync(userId)
            ?? throw new UserNotFoundException();

        var message = new Message
        {
            ChatId = chat.Id,
            SenderId = userId,
            Sender = user,
            Content = request.Content,
            SentAt = DateTime.UtcNow
        };

        var created = await _messageRepository.AddAsync(message);

        var payload = new MessageCreatedPayload(
            chat.Id,
            created.Id,
            created.SenderId,
            created.Content,
            created.SentAt
        );

        await _messagePublisher.PublishAsync(new MessageCreatedEvent(payload));

        return ResponseModel<MessageResponse>.Success(new MessageResponse(created));
    }

    public async Task<ResponseModel<IEnumerable<MessageResponse>>> GetChatMessagesAsync(Guid userId, Guid chatId)
    {
        var chat = await _chatRepository.GetByIdAsync(chatId)
            ?? throw new ChatNotFoundException();
        
        var member = await _chatMemberRepository.GetByUserAndChatAsync(userId, chatId) 
            ?? throw new NoAccessException();
        
        var messages = await _messageRepository.GetMessagesForChatAsync(chatId);

        return ResponseModel<IEnumerable<MessageResponse>>.Success(
            messages.Select(m => new MessageResponse(m))
        );
    }
    public async Task<ResponseModel<MessageResponse>> EditMessageAsync(Guid userId, Guid messageId, string newContent)
    {
        var message = await _messageRepository.GetByIdAsync(messageId)
            ?? throw new MessageNotFoundException();

        if (message.SenderId != userId)
            throw new NoAccessException();

        message.Content = newContent;
        message.EditedAt = DateTime.UtcNow;
        await _messageRepository.UpdateAsync(message);

        return ResponseModel<MessageResponse>.Success(new MessageResponse(message));
    }
    public async Task<ResponseModel<MessageResponse>> DeleteMessageAsync(Guid userId, Guid messageId)
    {
        var message = await _messageRepository.GetByIdAsync(messageId)
            ?? throw new MessageNotFoundException();

        if (message.SenderId != userId)
            throw new NoAccessException();

        await _messageRepository.RemoveAsync(message);

        return ResponseModel<MessageResponse>.Success(new MessageResponse(message));
    }

    public async Task<ResponseModel<MessageResponse>> TagMessageAsync(Guid userId, Guid messageId, string tag)
    {
        var message = await _messageRepository.GetByIdAsync(messageId)
            ?? throw new MessageNotFoundException();

        var member = await _chatMemberRepository.GetByUserAndChatAsync(userId, message.ChatId)
            ?? throw new NoAccessException();

        message.Tag = tag;
        await _messageRepository.UpdateAsync(message);

        return ResponseModel<MessageResponse>.Success(new MessageResponse(message));
    }
    public async Task<ResponseModel<IEnumerable<MessageResponse>>> GetMessagesByTagAsync(Guid userId, Guid chatId, string tag)
    {
        var member = await _chatMemberRepository.GetByUserAndChatAsync(userId, chatId)
            ?? throw new NoAccessException();

        var messages = await _messageRepository.GetMessagesByTagAsync(chatId, tag);

        return ResponseModel<IEnumerable<MessageResponse>>.Success(messages.Select(m => new MessageResponse(m)));
    }

    public Task<ResponseModel<MessageResponse>> EditMessageAsync(Guid messageId, string newContent, Guid editedBy)
    {
        throw new NotImplementedException();
    }

    Task<ResponseModel> IMessageService.DeleteMessageAsync(Guid messageId, Guid deletedBy)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> MarkMessageAsReadAsync(Guid chatId, Guid messageId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<IEnumerable<MessageResponse>>> GetMessagesPagedAsync(Guid chatId, int page, int pageSize)
    {
        throw new NotImplementedException();
    }
}