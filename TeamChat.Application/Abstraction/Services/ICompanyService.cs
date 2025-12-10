using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Company;
using TeamChat.Application.DTOs.CompanyUser;
using TeamChat.Domain.Entities;

namespace TeamChat.Application.Abstraction.Services;

public interface ICompanyService
{
    Task<ResponseModel<CompanyResponse>> CreateCompanyAsync(Guid directorId, CreateCompanyRequest request);
    Task<ResponseModel<CreateCompanyDepartmentResponse>> CreateCompanyDepartmentAsync(Guid userId, int companyId, CreateCompanyDepartmentRequest request);
    Task<ResponseModel<CreateCompanyPositionResponse>> CreateCompanyPositionAsync(CompanyUser? user, int companyId, CreateCompanyPositionRequest request);
    Task<ResponseModel<SetCompanyDetailsResponse>> SetCompanyDetailsAsync(int companyId, SetCompanyDetailsRequest request);
    Task<ResponseModel<CompanyUserResponse>> GetCompanyUserByUserIdAsync(Guid userId, int companyId);

}