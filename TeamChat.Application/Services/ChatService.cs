using TeamChat.Domain.Enums;
using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Chat;
using TeamChat.Application.DTOs.Message;
using TeamChat.Domain.Models.Exceptions;
using System.ComponentModel.DataAnnotations;
using TeamChat.Domain.Models.Exceptions.Company;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;
public class ChatService(IChatRepository chatRepository,
                         ICompanyUserRepository companyUserRepository,
                         IDepartmentRepository departmentRepository,
                         ITeamRepository teamRepository,
                         ICompanyRepository companyRepository,
                         IChatMemberRepository chatMemberRepository) : IChatService
{
    private readonly IChatRepository _chatRepository = chatRepository;
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

        List<Guid> participantIds = new();

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


}
