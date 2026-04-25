using TeamChat.Domain.Enums;
using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;
using TeamChat.Domain.Models.Exceptions;
using System.ComponentModel.DataAnnotations;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;

public class ChatService(
    IChatRepository chatRepository,
    ICompanyUserRepository companyUserRepository,
    IDepartmentRepository departmentRepository,
    ITeamRepository teamRepository,
    ICompanyRepository companyRepository,
    IMessageRepository messageRepository,
    IChatMemberRepository chatMemberRepository,
    IChatPositionAccessRepository chatPositionAccessRepository,
    IPositionHierarchyService positionHierarchyService,
    IDirectMessagePolicyService directMessagePolicyService,
    IPositionRepository positionRepository) : IChatService
{
    // ==================== СОЗДАНИЕ ЧАТОВ ====================

    public async Task<ResponseModel<CreateChatResponse>> CreateChatAsync(Guid userId, CreateChatRequest request)
    {
        var companyUser = await companyUserRepository.GetByUserAndCompany(userId, request.CompanyId)
            ?? throw new CompanyUserNotFoundException();

        if ((companyUser.Position.Permissions & PositionPermissions.CreateChat) == 0)
            throw new NoAccessException();

        List<Guid> participantIds = [];

        switch (request.Scope)
        {
            case ChatScope.Company:
                var companyUsers = await companyRepository.GetEmployeesAsync(request.CompanyId);
                participantIds.AddRange(companyUsers.Select(u => u.UserId));
                break;

            case ChatScope.Department:
                if (request.DepartmentId is null)
                    throw new ValidationException("DepartmentId is required for department chat");

                var dep = await departmentRepository.GetByIdAsync(request.DepartmentId.Value)
                    ?? throw new DepartmentNotFoundException();

                if (dep.CompanyId != request.CompanyId)
                    throw new ValidationException("Department does not belong to this company");

                var depUsers = await departmentRepository.GetEmployeesAsync(dep.Id);
                participantIds.AddRange(depUsers.Select(u => u.CompanyUser.UserId));
                break;

            case ChatScope.Team:
                if (request.TeamId is null)
                    throw new ValidationException("TeamId is required for team chat");

                var team = await teamRepository.GetByIdAsync(request.TeamId.Value)
                    ?? throw new TeamNotFoundException();

                if (team.CompanyId != request.CompanyId)
                    throw new ValidationException("Team does not belong to this company");

                var teamUsers = await teamRepository.GetEmployeesAsync(team.Id);
                participantIds.AddRange(teamUsers.Select(u => u.CompanyUser.UserId));
                break;

            default:
                throw new ValidationException("Invalid chat scope");
        }

        var chat = await chatRepository.AddAsync(new Chat
        {
            Name = request.Name,
            CompanyId = request.CompanyId,
            OwnerId = userId,
            Scope = request.Scope,
            DepartmentId = request.DepartmentId,
            TeamId = request.TeamId,
        });

        // Добавляем участников
        foreach (var id in participantIds.Distinct())
        {
            await chatMemberRepository.AddAsync(new ChatMember
            {
                ChatId = chat.Id,
                UserId = id,
            });
        }

        // Привязываем позиции если указаны
        if (request.PositionIds is { Count: > 0 })
        {
            foreach (var positionId in request.PositionIds.Distinct())
            {
                var position = await positionRepository.GetByIdAsync(positionId);
                if (position == null || position.CompanyId != request.CompanyId)
                    continue;

                await chatPositionAccessRepository.AddAsync(new ChatPositionAccess
                {
                    ChatId = chat.Id,
                    PositionId = positionId
                });

                // Добавляем всех юзеров на этой позиции как участников
                await AddPositionUsersToChat(chat.Id, positionId, participantIds);
            }
        }

        return ResponseModel<CreateChatResponse>.Success(
            new CreateChatResponse(chat, participantIds.Distinct().ToList()));
    }

    public async Task<ResponseModel<CreateChatResponse>> CreatePrivateChatAsync(
        Guid userId, Guid targetUserId, int companyId)
    {
        // Оба должны быть в компании
        var currentUser = await companyUserRepository.GetByUserAndCompany(userId, companyId)
            ?? throw new CompanyUserNotFoundException();

        var targetUser = await companyUserRepository.GetByUserAndCompany(targetUserId, companyId)
            ?? throw new ValidationException("Target user is not in this company");

        var canMessage = await directMessagePolicyService.CanMessageAsync(userId, targetUserId, companyId);
        if (!canMessage)
            throw new NoAccessException();

        var existingChat = await chatRepository.GetPrivateChatAsync(userId, targetUserId, companyId);
        if (existingChat != null)
            return ResponseModel<CreateChatResponse>.Success(
                new CreateChatResponse(existingChat, [userId, targetUserId]));

        var chat = await chatRepository.AddAsync(new Chat
        {
            Name = $"",  // имя формируется на фронте
            CompanyId = companyId,
            OwnerId = userId,
            Scope = ChatScope.Private,
        });

        await chatMemberRepository.AddAsync(new ChatMember { ChatId = chat.Id, UserId = userId });
        await chatMemberRepository.AddAsync(new ChatMember { ChatId = chat.Id, UserId = targetUserId });

        return ResponseModel<CreateChatResponse>.Success(
            new CreateChatResponse(chat, [userId, targetUserId]));
    }

    // ==================== ПРИВЯЗКА ПОЗИЦИЙ ====================

    public async Task<ResponseModel> AttachPositionToChatAsync(
        Guid userId, Guid chatId, int positionId, PositionPermissions? permissionOverride = null)
    {
        var chat = await chatRepository.GetByIdAsync(chatId)
            ?? throw new ChatNotFoundException();

        var companyUser = await companyUserRepository.GetByUserAndCompany(userId, chat.CompanyId)
            ?? throw new NoAccessException();

        // Только создатель чата или тот у кого есть ManageMembers
        if (chat.OwnerId != userId &&
            (companyUser.Position.Permissions & PositionPermissions.ManageMembers) == 0)
            throw new NoAccessException();

        var position = await positionRepository.GetByIdAsync(positionId)
            ?? throw new ValidationException("Position not found");

        if (position.CompanyId != chat.CompanyId)
            throw new ValidationException("Position does not belong to this company");

        // Нельзя привязывать позицию к приватному чату
        if (chat.Scope == ChatScope.Private)
            throw new ValidationException("Cannot attach position to private chat");

        // Проверяем, не привязана ли уже
        var existing = await chatPositionAccessRepository.GetByPositionAndChatAsync(positionId, chatId);
        if (existing != null)
            return ResponseModel.Fail("Position is already attached to this chat");

        await chatPositionAccessRepository.AddAsync(new ChatPositionAccess
        {
            ChatId = chatId,
            PositionId = positionId,
            PermissionOverride = permissionOverride
        });

        // Добавляем всех юзеров на этой позиции в чат
        await AddPositionUsersToChat(chatId, positionId, []);

        // Добавляем юзеров с родительских позиций (наследование вверх)
        var ancestors = await positionHierarchyService.GetAncestorPositionsAsync(positionId);
        foreach (var ancestor in ancestors)
        {
            await AddPositionUsersToChat(chatId, ancestor.Id, []);
        }

        return ResponseModel.Success("Position attached successfully");
    }

    public async Task<ResponseModel> DetachPositionFromChatAsync(
        Guid userId, Guid chatId, int positionId)
    {
        var chat = await chatRepository.GetByIdAsync(chatId)
            ?? throw new ChatNotFoundException();

        var companyUser = await companyUserRepository.GetByUserAndCompany(userId, chat.CompanyId)
            ?? throw new NoAccessException();

        if (chat.OwnerId != userId &&
            (companyUser.Position.Permissions & PositionPermissions.ManageMembers) == 0)
            throw new NoAccessException();

        var access = await chatPositionAccessRepository.GetByPositionAndChatAsync(positionId, chatId)
            ?? throw new ValidationException("Position is not attached to this chat");

        await chatPositionAccessRepository.RemoveAsync(access);

        // Удаляем из чата юзеров, которые были там только через эту позицию
        await RemoveOrphanedChatMembers(chatId);

        return ResponseModel.Success("Position detached successfully");
    }

    // ==================== СООБЩЕНИЯ ====================

    public async Task EditMessageAsync(Guid messageId, string newContent, Guid editedBy)
    {
        var message = await messageRepository.GetByIdAsync(messageId)
            ?? throw new ValidationException("Message not found");

        var companyUser = await companyUserRepository.GetByUserAndCompany(editedBy, message.Chat.CompanyId)
            ?? throw new NoAccessException();

        // Свои сообщения может редактировать любой, чужие — по правам
        if (message.SenderId != editedBy)
        {
            var permissions = await positionHierarchyService
                .GetEffectivePermissionsAsync(companyUser.PositionId, message.ChatId);

            if ((permissions & PositionPermissions.EditMessage) == 0)
                throw new NoAccessException();
        }

        message.Content = newContent;
        message.EditedAt = DateTime.UtcNow;
        await messageRepository.UpdateAsync(message);
    }

    public async Task DeleteMessageAsync(Guid messageId, Guid deletedBy)
    {
        var message = await messageRepository.GetByIdAsync(messageId)
            ?? throw new ValidationException("Message not found");

        var companyUser = await companyUserRepository.GetByUserAndCompany(deletedBy, message.Chat.CompanyId)
            ?? throw new NoAccessException();

        if (message.SenderId != deletedBy)
        {
            var permissions = await positionHierarchyService
                .GetEffectivePermissionsAsync(companyUser.PositionId, message.ChatId);

            if ((permissions & PositionPermissions.DeleteMessage) == 0)
                throw new NoAccessException();
        }

        await messageRepository.RemoveAsync(message);
    }

    public async Task MarkMessageAsReadAsync(Guid chatId, Guid messageId, Guid userId)
    {
        var chat = await chatRepository.GetByIdAsync(chatId)
            ?? throw new ValidationException("Chat not found");

        var chatMember = await chatMemberRepository.GetByUserAndChatAsync(userId, chatId)
            ?? throw new NoAccessException();

        var message = await messageRepository.GetByIdAsync(messageId)
            ?? throw new ValidationException("Message not found");

        if (message.ChatId != chatId)
            throw new ValidationException("Message does not belong to this chat");

        var existingRead = await messageRepository.GetReadStatusAsync(messageId, userId);
        if (existingRead == null)
        {
            await messageRepository.AddReadStatusAsync(new MessageReadStatus
            {
                MessageId = messageId,
                UserId = userId,
                ReadAt = DateTime.UtcNow
            });
        }
        else
        {
            existingRead.ReadAt = DateTime.UtcNow;
            await messageRepository.UpdateReadStatusAsync(existingRead);
        }
    }


    public async Task<ResponseModel<List<CompanyChatResponse>>> GetUserCompanyChatsAsync(
        Guid userId, int companyId)
    {
        var companyUser = await companyUserRepository.GetByUserAndCompany(userId, companyId);
        if (companyUser == null)
            throw new CompanyUserNotFoundException();

        var directChats = await chatRepository.GetUserCompanyChatsAsync(userId, companyId);

        var positionChats = await positionHierarchyService
            .GetAccessibleChatsAsync(companyUser.PositionId);

        var allChats = directChats
            .Concat(positionChats.Where(c => c.CompanyId == companyId))
            .DistinctBy(c => c.Id)
            .Select(c =>
            {
                string? otherUserName = null;
                string? otherUserAvatarUrl = null;

                if (c.Scope == ChatScope.Private && c.Members != null)
                {
                    var other = c.Members.FirstOrDefault(m => m.UserId != userId);
                    if (other?.User != null)
                    {
                        otherUserName = $"{other.User.FirstName} {other.User.LastName}".Trim();
                        otherUserAvatarUrl = other.User.AvatarUrl;
                    }
                }

                return new CompanyChatResponse(c.Id, c.Name, c.CreatedAt, c.Scope, otherUserName, otherUserAvatarUrl);
            })
            .ToList();

        return ResponseModel<List<CompanyChatResponse>>.Success(allChats);
    }

    // ==================== УПРАВЛЕНИЕ ====================

    public async Task<ResponseModel> DeleteChatAsync(Guid chatId, Guid requestedBy)
    {
        var chat = await chatRepository.GetByIdAsync(chatId)
            ?? throw new ChatNotFoundException();

        var companyUser = await companyUserRepository.GetByUserAndCompany(requestedBy, chat.CompanyId)
            ?? throw new NoAccessException();

        if (chat.OwnerId != requestedBy)
        {
            var permissions = await positionHierarchyService
                .GetEffectivePermissionsAsync(companyUser.PositionId, chatId);

            if ((permissions & PositionPermissions.DeleteChat) == 0)
                throw new NoAccessException();
        }

        await chatRepository.RemoveAsync(chat);
        return ResponseModel.Success("Chat deleted");
    }

    // ==================== ПРИВАТНЫЕ МЕТОДЫ ====================

    /// Добавляет всех юзеров на данной позиции в чат (если ещё не там)
    private async Task AddPositionUsersToChat(Guid chatId, int positionId, List<Guid> alreadyAdded)
    {
        var position = await positionRepository.GetByIdAsync(positionId);
        if (position == null) return;

        foreach (var companyUser in position.AssignedUsers)
        {
            if (alreadyAdded.Contains(companyUser.UserId))
                continue;

            var existingMember = await chatMemberRepository
                .GetByUserAndChatAsync(companyUser.UserId, chatId);

            if (existingMember == null)
            {
                await chatMemberRepository.AddAsync(new ChatMember
                {
                    ChatId = chatId,
                    UserId = companyUser.UserId
                });
                alreadyAdded.Add(companyUser.UserId);
            }
        }
    }

    /// Удаляет из чата тех, кто не имеет доступа ни через одну привязанную позицию
    private async Task RemoveOrphanedChatMembers(Guid chatId)
    {
        var chat = await chatRepository.GetByIdAsync(chatId);
        if (chat == null) return;

        // Собираем всех юзеров, которые должны быть в чате через позиции
        var positionAccess = await chatPositionAccessRepository.GetByChatAsync(chatId);
        var allowedUserIds = new HashSet<Guid>();

        foreach (var access in positionAccess)
        {
            // Юзеры на этой позиции
            foreach (var cu in access.Position.AssignedUsers)
                allowedUserIds.Add(cu.UserId);

            // Юзеры на родительских позициях (наследование)
            var ancestors = await positionHierarchyService
                .GetAncestorPositionsAsync(access.PositionId);

            foreach (var ancestor in ancestors)
            {
                var ancestorPos = await positionRepository.GetByIdAsync(ancestor.Id);
                if (ancestorPos?.AssignedUsers == null) continue;
                foreach (var cu in ancestorPos.AssignedUsers)
                    allowedUserIds.Add(cu.UserId);
            }
        }

        // Владелец чата остаётся всегда
        allowedUserIds.Add(chat.OwnerId);

        // Удаляем тех кто не в списке (но не трогаем приватные чаты)
        if (chat.Scope == ChatScope.Private) return;

        var members = chat.Members.ToList();
        foreach (var member in members)
        {
            if (!allowedUserIds.Contains(member.UserId))
            {
                await chatMemberRepository.RemoveAsync(member);
            }
        }
    }
}