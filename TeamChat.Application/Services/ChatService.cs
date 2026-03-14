using TeamChat.Domain.Enums;
using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;
using TeamChat.Domain.Models.Exceptions;
using System.ComponentModel.DataAnnotations;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;

public class ChatService(IChatRepository chatRepository,
                         ICompanyUserRepository companyUserRepository,
                         IDepartmentRepository departmentRepository,
                         ITeamRepository teamRepository,
                         ICompanyRepository companyRepository,
                         IMessageRepository messageRepository,
                         IChatMemberRepository chatMemberRepository) : IChatService
{
    private readonly IChatRepository _chatRepository = chatRepository;
    private readonly IMessageRepository _messageRepository = messageRepository;
    private readonly ICompanyUserRepository _companyUserRepository = companyUserRepository;
    private readonly IDepartmentRepository _departmentRepository = departmentRepository;
    private readonly ITeamRepository _teamRepository = teamRepository;
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IChatMemberRepository _chatMemberRepository = chatMemberRepository;

    public async Task<ResponseModel<ChatResponse>> CreateChatAsync(Guid userId, CreateChatRequest request)
    {
        var companyUser = await _companyUserRepository.GetByUserAndCompany(userId, request.CompanyId)
            ?? throw new CompanyUserNotFoundException();

        if ((companyUser.Position.Permissions & PositionPermissions.CreateChat) == 0)
            throw new NoAccessException();

        List<Guid> participantIds = [];

        switch (request.Scope)
        {
            case ChatScope.Company:
                var companyUsers = await _companyRepository.GetEmployeesAsync(request.CompanyId);
                participantIds.AddRange(companyUsers.Select(u => u.UserId));
                break;

            case ChatScope.Department:
                if (request.DepartmentId is null)
                    throw new ValidationException("DepartmentId is required for department chat");

                var dep = await _departmentRepository.GetByIdAsync(request.DepartmentId.Value)
                    ?? throw new DepartmentNotFoundException();

                if (dep.CompanyId != request.CompanyId)
                    throw new ValidationException("Department does not belong to this company");

                var depUsers = await _departmentRepository.GetEmployeesAsync(dep.Id);
                participantIds.AddRange(depUsers.Select(u => u.CompanyUser.UserId));
                break;

            case ChatScope.Team:
                if (request.TeamId is null)
                    throw new ValidationException("TeamId is required for team chat");

                var team = await _teamRepository.GetByIdAsync(request.TeamId.Value)
                    ?? throw new TeamNotFoundException();

                if (team.CompanyId != request.CompanyId)
                    throw new ValidationException("Team does not belong to this company");

                var teamUsers = await _teamRepository.GetEmployeesAsync(team.Id);
                participantIds.AddRange(teamUsers.Select(u => u.CompanyUser.UserId));
                break;

            default:
                throw new ValidationException("Invalid chat scope");
        }

        var chat = await _chatRepository.AddAsync(new Chat
        {
            Name = request.Name,
            CompanyId = request.CompanyId,
            OwnerId = userId,
            DepartmentId = request.DepartmentId,
            TeamId = request.TeamId,
        });

        foreach (var id in participantIds.Distinct())
        {
            var chatMember = new ChatMember
            {
                ChatId = chat.Id,
                UserId = id,
            };
            await _chatMemberRepository.AddAsync(chatMember);
        }

        return ResponseModel<ChatResponse>.Success(new ChatResponse(chat));
    }

    public async Task<ResponseModel<ChatResponse>> AddUserToChatAsync(Guid userId, Guid chatId, Guid targetUserId)
    {
        var chat = await _chatRepository.GetByIdAsync(chatId) ?? throw new ChatNotFoundException();

        var companyUser = await _companyUserRepository.GetByUserAndCompany(userId, chat.CompanyId)
            ?? throw new NoAccessException();

        if ((companyUser.Position.Permissions & PositionPermissions.AddChatMember) == 0)
            throw new NoAccessException();

        var existingMember = await _chatMemberRepository.GetByUserAndChatAsync(targetUserId, chatId);
        if (existingMember != null)
            return ResponseModel<ChatResponse>.Fail("User already in chat");

        var chatMember = new ChatMember
        {
            ChatId = chatId,
            UserId = targetUserId
        };
        await _chatMemberRepository.AddAsync(chatMember);

        return ResponseModel<ChatResponse>.Success(new ChatResponse(chat));
    }

    public async Task<ResponseModel<ChatResponse>> RemoveUserFromChatAsync(Guid userId, Guid chatId, Guid targetUserId)
    {
        var chat = await _chatRepository.GetByIdAsync(chatId) ?? throw new ChatNotFoundException();

        var companyUser = await _companyUserRepository.GetByUserAndCompany(userId, chat.CompanyId)
            ?? throw new NoAccessException();

        if ((companyUser.Position.Permissions & PositionPermissions.RemoveChatMember) == 0)
            throw new NoAccessException();

        var chatMember = await _chatMemberRepository.GetByUserAndChatAsync(targetUserId, chatId)
            ?? throw new ValidationException("User is not a member of this chat");

        await _chatMemberRepository.RemoveAsync(chatMember);

        return ResponseModel<ChatResponse>.Success(new ChatResponse(chat));
    }

    public async Task<ResponseModel<ChatResponse>> SetChatTopicAsync(Guid userId, Guid chatId, string topic)
    {
        var chat = await _chatRepository.GetByIdAsync(chatId) ?? throw new ChatNotFoundException();

        if (chat.OwnerId != userId)
            throw new NoAccessException();

        chat.Topic = topic;
        await _chatRepository.UpdateAsync(chat);

        return ResponseModel<ChatResponse>.Success(new ChatResponse(chat));
    }

    public async Task<ResponseModel<ChatResponse>> PinMessageAsync(Guid userId, Guid chatId, Guid messageId)
    {
        var chat = await _chatRepository.GetByIdAsync(chatId) ?? throw new ChatNotFoundException();

        var member = await _chatMemberRepository.GetByUserAndChatAsync(userId, chatId)
            ?? throw new NoAccessException();

        var message = await _messageRepository.GetByIdAsync(messageId)
            ?? throw new ValidationException("Message not found");

        chat.PinnedMessageId = message.Id;
        await _chatRepository.UpdateAsync(chat);

        return ResponseModel<ChatResponse>.Success(new ChatResponse(chat));
    }

    public async Task EditMessageAsync(Guid messageId, string newContent, Guid editedBy)
    {
        var message = await _messageRepository.GetByIdAsync(messageId)
                      ?? throw new ValidationException("Message not found");

        var companyUser = await _companyUserRepository.GetByUserAndCompany(editedBy, message.Chat.CompanyId)
                           ?? throw new NoAccessException();

        var permissions = companyUser.Position.Permissions;

        if (message.SenderId != editedBy && (permissions & PositionPermissions.EditMessage) == 0)
            throw new NoAccessException();

        message.Content = newContent;
        message.EditedAt = DateTime.UtcNow;

        await _messageRepository.UpdateAsync(message);
    }

    public async Task DeleteMessageAsync(Guid messageId, Guid deletedBy)
    {
        var message = await _messageRepository.GetByIdAsync(messageId)
                      ?? throw new ValidationException("Message not found");

        var companyUser = await _companyUserRepository.GetByUserAndCompany(deletedBy, message.Chat.CompanyId)
                           ?? throw new NoAccessException();

        var permissions = companyUser.Position.Permissions;

        if (message.SenderId != deletedBy && (permissions & PositionPermissions.DeleteMessage) == 0)
            throw new NoAccessException();

        await _messageRepository.RemoveAsync(message);
    }

    public async Task MarkMessageAsReadAsync(Guid chatId, Guid messageId, Guid userId)
    {
        // Проверяем, что чат существует
        var chat = await _chatRepository.GetByIdAsync(chatId)
                   ?? throw new ValidationException("Chat not found");

        // Проверяем, что пользователь состоит в чате
        var chatMember = await _chatMemberRepository.GetByUserAndChatAsync(userId, chatId)
                         ?? throw new NoAccessException();

        // Проверяем, что сообщение существует
        var message = await _messageRepository.GetByIdAsync(messageId)
                      ?? throw new ValidationException("Message not found");

        // Проверяем, что сообщение относится к этому чату
        if (message.ChatId != chatId)
            throw new ValidationException("Message does not belong to this chat");

        // Добавляем/обновляем статус прочтения
        var existingRead = await _messageRepository.GetReadStatusAsync(messageId, userId);
        if (existingRead == null)
        {
            await _messageRepository.AddReadStatusAsync(new MessageReadStatus
            {
                MessageId = messageId,
                UserId = userId,
                ReadAt = DateTime.UtcNow
            });
        }
        else
        {
            existingRead.ReadAt = DateTime.UtcNow;
            await _messageRepository.UpdateReadStatusAsync(existingRead);
        }
    }

    public Task<ResponseModel<IEnumerable<ChatResponse>>> GetUserChatsAsync(Guid userId)
    {
        throw new NotImplementedException();
    }

    Task<ResponseModel> IChatService.AddUserToChatAsync(Guid chatId, Guid addedBy, Guid newUserId)
    {
        throw new NotImplementedException();
    }

    Task<ResponseModel> IChatService.RemoveUserFromChatAsync(Guid chatId, Guid removedBy, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> AssignChatAdminAsync(Guid chatId, Guid userId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> DeleteChatAsync(Guid chatId, Guid requestedBy)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModel<List<CompanyChatResponse>>> GetUserCompanyChatsAsync(Guid userId, int companyId)
    {
        var chats = await _chatRepository.GetUserCompanyChatsAsync(userId, companyId);

        var response = chats
            .Select(c => new CompanyChatResponse(c.Id, c.Name, c.CreatedAt))
            .ToList();

        return ResponseModel<List<CompanyChatResponse>>.Success(response);
    }
}