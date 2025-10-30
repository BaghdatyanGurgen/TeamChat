using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.Messaging;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Company;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Enums;
using TeamChat.Messaging.Contracts.Events.Company;

namespace TeamChat.Application.Services;

public class CompanyService(
    ICompanyRepository companyRepository,
    IUserRepository userRepository,
    IMessagePublisher messagePublisher,
    IPositionRepository positionRepository,
    ICompanyUserRepository companyUserRepository) : ICompanyService
{
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly IMessagePublisher _messagePublisher = messagePublisher;
    private readonly ICompanyUserRepository _companyUserRepository = companyUserRepository;

    public async Task<ResponseModel<CompanyUserResponse>> AddUserToCompanyAsync(AddUserToCompanyRequest request)
    {
        var model = new CompanyUser
        {
            CompanyId = request.CompanyId,
            UserId = request.UserId,
            PositionId = request.PositionId,
            JoinedAt = DateTime.UtcNow,
        };
        CompanyUser companyUser = await _companyUserRepository.Add(model);

        var response = new CompanyUserResponse(companyUser);

        var mqEvent = new UserAddedEvent(new UserAddedEventPayload(request.CompanyId, request.UserId, request.PositionId));

        await _messagePublisher.PublishAsync(mqEvent);

        return ResponseModel<CompanyUserResponse>.Success(response);
    }

    public Task<bool> CheckPermissionAsync(CheckPermissionRequest request)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModel<CompanyResponse>> CreateCompanyAsync(CreateCompanyRequest request)
    {
        var company = new Company
        {
            Name = request.Name,
            DirectorId = request.DirectorId
        };
        var result = await _companyRepository.AddAsync(company);
        return ResponseModel<CompanyResponse>.Success(new CompanyResponse(result));
    }

    public async Task<ResponseModel<PositionResponse>> CreatePositionAsync(CreatePositionRequest request)
    {
        var company = await _companyRepository.GetByIdAsync(request.CompanyId);

        if (company == null)
            return ResponseModel<PositionResponse>.Fail("Company not found");

        var position = new Position
        {
            CompanyId = request.CompanyId,
            Company = company,
            CreatedByUserId = request.AdminUserId,
            Title = request.Title,
            Permissions = request.Permissions.Aggregate(PositionPermissions.None, (acc, p) => acc | p),
            InviteCode = GenerateInviteCode()
        };

        _positionRepository.AddAsync(position);


        return ResponseModel<PositionResponse>.Success(new PositionResponse(position));
    }

    public async Task<ResponseModel> DeactivateUserAsync(DeactivateUserRequest request)
    {
        var isDeactivated = await _userRepository.DeactivateUserInCompanyAsync(request.UserId, request.CompanyId);

        if (!isDeactivated)
            return ResponseModel.Fail("User deactivation failed");

        return ResponseModel.Success("Account is diactivated");
    }

    public Task<ResponseModel> DeletePositionAsync(DeletePositionRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<NullableCompanyResponse>> GetByIdAsync(CompanyByIdRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<GetUserCompaniesResponse>> GetUserCompaniesAsync(UserCompaniesRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<CompanyResponse>> UpdateCompanyAsync(UpdateCompanyRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<PositionResponse>> UpdatePositionAsync(UpdatePositionRequest request)
    {
        throw new NotImplementedException();
    }
    private static string GenerateInviteCode() => Guid.NewGuid().ToString("N")[..8].ToUpper();

}
