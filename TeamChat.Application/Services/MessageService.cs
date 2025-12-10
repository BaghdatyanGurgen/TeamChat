
using TeamChat.Application.Abstraction.Infrastructure.Messaging;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;
using TeamChat.Application.DTOs.Message;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Models.Exceptions;
using TeamChat.Domain.Models.Exceptions.Company;
using TeamChat.Messaging.Contracts.Message;

namespace TeamChat.Application.Services;

public class MessageService(IMessageRepository messageRepository,
                            IChatRepository chatRepository,
                            IChatMemberRepository chatMemberRepository,
                            IMessagePublisher messagePublisher) : IMessageService
{
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IChatMemberRepository _chatMemberRepository = chatMemberRepository;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;

    public async Task<ResponseModel<MessageResponse>> CreateMessageAsync(Guid userId, CreateMessageRequest request)
    {
        var chat = await _chatRepository.GetByIdAsync(request.ChatId)
            ?? throw new ChatNotFoundException();
        
        _ = await _chatMemberRepository.GetByUserAndChatAsync(userId, request.ChatId)
            ?? throw new NoAccessException();
        var message = new Message
        {
            ChatId = chat.Id,
            SenderId = userId,
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
}