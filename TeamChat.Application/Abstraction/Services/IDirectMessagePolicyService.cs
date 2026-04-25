using TeamChat.Domain.Enums;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.CompanyUser;

namespace TeamChat.Application.Abstraction.Services;

public interface IDirectMessagePolicyService
{
    Task<ResponseModel<List<CompanyMemberResponse>>> GetAvailableDirectContactsAsync(Guid userId, int companyId);

    Task<bool> CanMessageAsync(Guid userId, Guid targetUserId, int companyId);

    Task<ResponseModel> SetPolicyAsync(Guid userId, int companyId, DirectMessagePolicy policy);

    Task<ResponseModel<DirectMessagePolicy>> GetPolicyAsync(int companyId);
}