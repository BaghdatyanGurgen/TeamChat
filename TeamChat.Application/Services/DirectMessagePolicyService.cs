using TeamChat.Domain.Enums;
using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Domain.Models.Exceptions;
using TeamChat.Application.DTOs.CompanyUser;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;

public class DirectMessagePolicyService(
    ICompanyRepository companyRepository,
    ICompanyUserRepository companyUserRepository,
    IDepartmentRepository departmentRepository,
    ITeamRepository teamRepository,
    IPositionRepository positionRepository) : IDirectMessagePolicyService
{
    public async Task<ResponseModel<List<CompanyMemberResponse>>> GetAvailableDirectContactsAsync(
        Guid userId, int companyId)
    {
        var company = await companyRepository.GetByIdAsync(companyId)
            ?? throw new CompanyNotFoundException();

        var currentUser = await companyUserRepository.GetByUserAndCompany(userId, companyId)
            ?? throw new CompanyUserNotFoundException();

        var allEmployees = (await companyRepository.GetEmployeesAsync(companyId))
            .Where(cu => cu.UserId != userId)
            .ToList();

        List<CompanyUser> allowedUsers;

        switch (company.DmPolicy)
        {
            case DirectMessagePolicy.Anyone:
                allowedUsers = allEmployees;
                break;

            case DirectMessagePolicy.SameGroup:
                allowedUsers = await GetSameGroupUsersAsync(currentUser, allEmployees);
                break;

            case DirectMessagePolicy.DirectHierarchy:
                allowedUsers = await GetDirectHierarchyUsersAsync(currentUser, allEmployees);
                break;

            case DirectMessagePolicy.GroupAndHierarchy:
                var groupUsers = await GetSameGroupUsersAsync(currentUser, allEmployees);
                var hierarchyUsers = await GetDirectHierarchyUsersAsync(currentUser, allEmployees);
                allowedUsers = groupUsers
                    .Concat(hierarchyUsers)
                    .DistinctBy(cu => cu.UserId)
                    .ToList();
                break;

            default:
                allowedUsers = allEmployees;
                break;
        }

        // Директор всегда может писать всем и все могут писать директору
        if (company.DirectorId == userId)
        {
            allowedUsers = allEmployees;
        }
        else
        {
            var director = allEmployees.FirstOrDefault(cu => cu.UserId == company.DirectorId);
            if (director != null && !allowedUsers.Any(cu => cu.UserId == company.DirectorId))
            {
                allowedUsers.Add(director);
            }
        }

        var result = allowedUsers.Select(cu => new CompanyMemberResponse(
            cu.UserId,
            cu.User.FirstName ?? "",
            cu.User.LastName ?? "",
            cu.User.AvatarUrl,
            cu.Position.Title
        )).ToList();

        return ResponseModel<List<CompanyMemberResponse>>.Success(result);
    }

    public async Task<bool> CanMessageAsync(Guid userId, Guid targetUserId, int companyId)
    {
        var company = await companyRepository.GetByIdAsync(companyId);
        if (company == null) return false;

        if (company.DirectorId == userId || company.DirectorId == targetUserId)
            return true;

        if (company.DmPolicy == DirectMessagePolicy.Anyone)
            return true;

        var currentUser = await companyUserRepository.GetByUserAndCompany(userId, companyId);
        var targetUser = await companyUserRepository.GetByUserAndCompany(targetUserId, companyId);
        if (currentUser == null || targetUser == null) return false;

        var targetList = new List<CompanyUser> { targetUser };

        return company.DmPolicy switch
        {
            DirectMessagePolicy.SameGroup =>
                (await GetSameGroupUsersAsync(currentUser, targetList)).Any(),

            DirectMessagePolicy.DirectHierarchy =>
                (await GetDirectHierarchyUsersAsync(currentUser, targetList)).Any(),

            DirectMessagePolicy.GroupAndHierarchy =>
                (await GetSameGroupUsersAsync(currentUser, targetList)).Any() ||
                (await GetDirectHierarchyUsersAsync(currentUser, targetList)).Any(),

            _ => true
        };
    }

    public async Task<ResponseModel> SetPolicyAsync(
        Guid userId, int companyId, DirectMessagePolicy policy)
    {
        var company = await companyRepository.GetByIdAsync(companyId)
            ?? throw new CompanyNotFoundException();

        var companyUser = await companyUserRepository.GetByUserAndCompany(userId, companyId)
            ?? throw new CompanyUserNotFoundException();

        if (company.DirectorId != userId &&
            (companyUser.Position.Permissions & PositionPermissions.ManageDirectMessagePolicy) == 0)
            throw new NoAccessException();

        company.DmPolicy = policy;
        await companyRepository.UpdateAsync(company);

        return ResponseModel.Success("Direct message policy updated successfully");
    }

    public async Task<ResponseModel<DirectMessagePolicy>> GetPolicyAsync(int companyId)
    {
        var company = await companyRepository.GetByIdAsync(companyId)
            ?? throw new CompanyNotFoundException();

        return ResponseModel<DirectMessagePolicy>.Success(company.DmPolicy);
    }

    // ==================== ПРИВАТНЫЕ МЕТОДЫ ====================

    private async Task<List<CompanyUser>> GetSameGroupUsersAsync(
        CompanyUser currentUser, List<CompanyUser> candidates)
    {
        var currentDepartments = await GetUserDepartmentIdsAsync(currentUser.Id);
        var currentTeams = await GetUserTeamIdsAsync(currentUser.Id);

        var result = new List<CompanyUser>();

        foreach (var candidate in candidates)
        {
            // Общие департаменты
            if (currentDepartments.Count > 0)
            {
                var candidateDepartments = await GetUserDepartmentIdsAsync(candidate.Id);
                if (currentDepartments.Intersect(candidateDepartments).Any())
                {
                    result.Add(candidate);
                    continue;
                }
            }

            // Общие команды
            if (currentTeams.Count > 0)
            {
                var candidateTeams = await GetUserTeamIdsAsync(candidate.Id);
                if (currentTeams.Intersect(candidateTeams).Any())
                {
                    result.Add(candidate);
                    continue;
                }
            }

            // Позиции в одном департаменте
            if (currentUser.Position.DepartmentId != null &&
                candidate.Position.DepartmentId == currentUser.Position.DepartmentId)
            {
                result.Add(candidate);
            }
        }

        return result;
    }

    private async Task<List<CompanyUser>> GetDirectHierarchyUsersAsync(
        CompanyUser currentUser, List<CompanyUser> candidates)
    {
        var allowedPositionIds = new HashSet<int>();

        // Прямой начальник
        if (currentUser.Position.ParentPositionId.HasValue)
        {
            allowedPositionIds.Add(currentUser.Position.ParentPositionId.Value);
        }

        // Прямые подчинённые
        var children = await positionRepository.GetChildrenAsync(currentUser.PositionId);
        foreach (var child in children)
        {
            allowedPositionIds.Add(child.Id);
        }

        return candidates
            .Where(cu => allowedPositionIds.Contains(cu.PositionId))
            .ToList();
    }

    private async Task<List<int>> GetUserDepartmentIdsAsync(int companyUserId)
    {
        var members = await departmentRepository.GetByCompanyUserAsync(companyUserId);
        return members.Select(d => d.DepartmentId).ToList();
    }

    private async Task<List<int>> GetUserTeamIdsAsync(int companyUserId)
    {
        var members = await teamRepository.GetByCompanyUserAsync(companyUserId);
        return members.Select(t => t.TeamId).ToList();
    }
}