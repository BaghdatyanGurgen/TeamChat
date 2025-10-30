using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Company;

namespace TeamChat.Application.Abstraction.Services;

public interface ICompanyService
{
    Task<ResponseModel<CompanyResponse>> CreateCompanyAsync(CreateCompanyRequest request);

    Task<ResponseModel<GetUserCompaniesResponse>> GetUserCompaniesAsync(UserCompaniesRequest request);

    Task<ResponseModel<NullableCompanyResponse>> GetByIdAsync(CompanyByIdRequest request);

    Task<ResponseModel<CompanyResponse>> UpdateCompanyAsync(UpdateCompanyRequest request);

    Task<ResponseModel<CompanyUserResponse>> AddUserToCompanyAsync(AddUserToCompanyRequest request);

    Task<ResponseModel> DeactivateUserAsync(DeactivateUserRequest request);

    Task<ResponseModel<PositionResponse>> CreatePositionAsync(CreatePositionRequest request);

    Task<ResponseModel<PositionResponse>> UpdatePositionAsync(UpdatePositionRequest request);

    Task<ResponseModel> DeletePositionAsync(DeletePositionRequest request);

    Task<bool> CheckPermissionAsync(CheckPermissionRequest request);
}