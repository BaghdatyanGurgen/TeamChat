using TeamChat.Domain.Enums;
using TeamChat.Domain.Entities;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Company;
using TeamChat.Domain.Models.Exceptions;
using TeamChat.Application.DTOs.CompanyUser;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.Abstraction.Infrastructure.File;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;

namespace TeamChat.Application.Services;

public class CompanyService(ICompanyRepository companyRepository,
                            IDepartmentRepository deprtmentRepository,
                            IPositionRepository positionRepository,
                            ICompanyUserRepository companyUserRepository,
                            IFileService fileService) : ICompanyService
{
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IFileService _fileService = fileService;
    private readonly IDepartmentRepository _deprtmentRepository = deprtmentRepository;
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly ICompanyUserRepository _companyUserRepository = companyUserRepository;

    public async Task<ResponseModel<CompanyResponse>> CreateCompanyAsync(Guid directorId, CreateCompanyRequest request)
    {
        var company = new Company
        {
            Name = request.Name,
            DirectorId = directorId,
        };

        var createdCompany = await _companyRepository.AddAsync(company);

        var directorPosition = await _positionRepository.AddAsync(new Position
        {
            CompanyId = createdCompany.Id,
            CreatedByUserId = directorId,
            Title = "Director",
            Permissions = PositionPermissions.All
        }) ?? throw new Exception("Cannot create director position");

        _ = await _companyUserRepository.AddAsync(new CompanyUser
        {
            UserId = directorId,
            CompanyId = createdCompany.Id,
            PositionId = directorPosition.Id,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        });

        return ResponseModel<CompanyResponse>.Success(new CompanyResponse(createdCompany));
    }
    public async Task<ResponseModel<SetCompanyDetailsResponse>> SetCompanyDetailsAsync(int companyId, SetCompanyDetailsRequest request)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        if (company == null)
            return ResponseModel<SetCompanyDetailsResponse>.Fail("Company not found");

        string? logoUrl = null;

        if (request.LogoFile != null)
        {
            logoUrl = await _fileService.UploadFileAsync(
                request.LogoFile,
                $"companies/{company.Id}"
            );
        }

        company.Description = request.Description;

        if (logoUrl != null)
            company.LogoUrl = logoUrl;

        await _companyRepository.UpdateAsync(company);

        return ResponseModel<SetCompanyDetailsResponse>.Success(new SetCompanyDetailsResponse(company.Id, company.Name, company.Description, company.LogoUrl));
    }
    public async Task<ResponseModel<CreateCompanyDepartmentResponse>> CreateCompanyDepartmentAsync(Guid userId, int companyId, CreateCompanyDepartmentRequest request)
    {
        var userCompany = await _companyUserRepository.GetByUserAndCompany(userId, companyId) ?? throw new CompanyUserNotFoundException();

        if ((userCompany.Position.Permissions & PositionPermissions.CreateDepartment) == 0)
            throw new NoAccessException();

        var department = await _deprtmentRepository.AddAsync(new Department
        {
            Name = request.Name,
            Description = request.Description,
            CompanyId = companyId
        }) ?? throw new CannotCreatePossitionException();

        var companyUserResponse = new CompanyUserResponse(userCompany);
        
        _ = await CreateCompanyPositionAsync(companyUserResponse,
                                             companyId,
                                             new CreateCompanyPositionRequest(request.Name + "Head",
                                                                              PositionPermissions.CreateChat | PositionPermissions.CreatePosition));

        var result = new CreateCompanyDepartmentResponse(department);

        return ResponseModel<CreateCompanyDepartmentResponse>.Success(result);
    }

    public async Task<ResponseModel<CreateCompanyPositionResponse>> CreateCompanyPositionAsync(CompanyUserResponse user, int companyId, CreateCompanyPositionRequest request)
    {
        if (user is null)
            throw new CompanyUserNotFoundException();

        var position = await _positionRepository.GetByIdAsync(user.PositionId) 
            ?? throw new CompanyUserNotFoundException();

        if ((position.Permissions & PositionPermissions.CreatePosition) == 0)
            throw new NoAccessException();

        var company = await _companyRepository.GetByIdAsync(user.CompanyId)
            ?? throw new CompanyUserNotFoundException();

        if (company.DirectorId != user.UserId)
            throw new NoAccessException();

        var department = await _positionRepository.AddAsync(new Position
        {
            CompanyId = companyId,
            CreatedByUserId = user.UserId,
            Title = request.Title,
            InviteCode = GenerateInviteCode(),
            Permissions = request.Permissions,
            ParentPositionId = user.PositionId
        }) ?? throw new CannotCreatePossitionException();

        var result = new CreateCompanyPositionResponse(department);

        return ResponseModel<CreateCompanyPositionResponse>.Success(result);
    }
    public async Task<ResponseModel<CompanyUserResponse>> GetCompanyUserByUserIdAsync(Guid userId, int companyId)
    {
        var companyUser = await _companyUserRepository.GetByUserAndCompany(userId, companyId);

        return companyUser is null
            ? throw new CompanyUserNotFoundException()
            : ResponseModel<CompanyUserResponse>.Success(new CompanyUserResponse(companyUser));
    }

    private static string GenerateInviteCode() => Guid.NewGuid().ToString("N")[..8].ToUpper();
}