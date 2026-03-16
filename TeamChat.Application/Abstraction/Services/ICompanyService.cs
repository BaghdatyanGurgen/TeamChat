using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Company;
using TeamChat.Application.DTOs.CompanyUser;

namespace TeamChat.Application.Abstraction.Services;

public interface ICompanyService
{
    Task<ResponseModel<CompanyResponse>> CreateCompanyAsync(Guid directorId, CreateCompanyRequest request);
    Task<ResponseModel<CreateCompanyDepartmentResponse>> CreateCompanyDepartmentAsync(Guid userId, int companyId, CreateCompanyDepartmentRequest request);
    Task<ResponseModel<CreateCompanyPositionResponse>> CreateCompanyPositionAsync(CompanyUserResponse user, int companyId, CreateCompanyPositionRequest request);
    Task<ResponseModel<SetCompanyDetailsResponse>> SetCompanyDetailsAsync(int companyId, SetCompanyDetailsRequest request);
    Task<ResponseModel<CompanyUserResponse>> GetCompanyUserByUserIdAsync(Guid userId, int companyId);
    Task<ResponseModel<CompanyResponse>> GetCompanyByIdAsync(int companyId);
    Task<ResponseModel<List<CompanyResponse>>> GetUserCompaniesAsync(Guid userId);
    Task<ResponseModel> DeleteDepartmentAsync(int departmentId, Guid requestedBy);
    Task<ResponseModel> DeletePositionAsync(int positionId, Guid requestedBy);
    Task<ResponseModel> InviteUserAsync(int companyId, Guid invitedBy, string email, int? positionId);
    Task<ResponseModel> ChangeUserRoleAsync(int companyId, Guid userId, string newRole, Guid changedBy);
    Task<ResponseModel> ArchiveCompanyAsync(int companyId, Guid requestedBy);
    Task<ResponseModel<CompanyResponse>> JoinCompanyByInviteAsync(Guid userId, JoinCompanyByInviteRequest request);
    Task<ResponseModel<List<PositionWithInviteResponse>>> GetUserPositionsAsync(Guid userId, int companyId);
    Task<ResponseModel<List<CreateCompanyDepartmentResponse>>> GetCompanyDepartmentsAsync(int companyId);
}