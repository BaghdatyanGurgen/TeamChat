using Microsoft.EntityFrameworkCore;
using TeamChat.Application.Abstraction.Infrastructure.File;
using TeamChat.Application.Abstraction.Infrastructure.Repositories;
using TeamChat.Application.Abstraction.Services;
using TeamChat.Application.DTOs;
using TeamChat.Application.DTOs.Company;
using TeamChat.Application.DTOs.CompanyUser;
using TeamChat.Domain.Entities;
using TeamChat.Domain.Enums;
using TeamChat.Domain.Models.Exceptions;

namespace TeamChat.Application.Services;

public class CompanyService(ICompanyRepository companyRepository,
                            IDepartmentRepository deprtmentRepository,
                            IPositionRepository positionRepository,
                            ICompanyUserRepository companyUserRepository,
                            IChatRepository chatRepository,
                            IChatMemberRepository chatMemberRepository,
                            IFileService fileService) : ICompanyService
{
    private readonly ICompanyRepository _companyRepository = companyRepository;
    private readonly IFileService _fileService = fileService;
    private readonly IDepartmentRepository _deprtmentRepository = deprtmentRepository;
    private readonly IPositionRepository _positionRepository = positionRepository;
    private readonly ICompanyUserRepository _companyUserRepository = companyUserRepository;
    private readonly IChatMemberRepository _chatMemberRepository = chatMemberRepository;
    private readonly IChatRepository _chatRepository = chatRepository;

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

        var companyUser = await _companyUserRepository.AddAsync(new CompanyUser
        {
            UserId = directorId,
            CompanyId = createdCompany.Id,
            PositionId = directorPosition.Id,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        });


        var companyChat = await _chatRepository.AddAsync(new Chat
        {
            CompanyId = createdCompany.Id,
            Name = $"{createdCompany.Name} General",
            OwnerId = directorId,
            CreatedAt = DateTime.UtcNow
        });

        _ = await _chatMemberRepository.AddAsync(new ChatMember
        {
            ChatId = companyChat.Id,
            UserId = directorId
        });

        return ResponseModel<CompanyResponse>.Success(new CompanyResponse(createdCompany));
    }
    public async Task<ResponseModel<SetCompanyDetailsResponse>> SetCompanyDetailsAsync(int companyId, SetCompanyDetailsRequest request)
    {
        var company = await _companyRepository.GetByIdAsync(companyId);
        if (company == null)
            return ResponseModel<SetCompanyDetailsResponse>.Fail("Company not found");

        company.Description = request.Description;

        if (request.LogoFile != null)
        {
            var (relativeUrl, _) = await _fileService.UploadFileAsync(request.LogoFile, $"companies/{company.Id}");
            var parts = relativeUrl.TrimStart('/').Split('/', 3);
            var folder = parts.Length >= 3 ? parts[1] : $"companies/{company.Id}";
            var fileName = parts.Length >= 3 ? parts[2] : relativeUrl;
            company.LogoUrl = $"/api/files/{folder}/{fileName}";
        }

        await _companyRepository.UpdateAsync(company);

        return ResponseModel<SetCompanyDetailsResponse>.Success(
            new SetCompanyDetailsResponse(company.Id, company.Name, company.Description, company.LogoUrl));
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
        }) ?? throw new CannotCreatePositionException();

        var companyUserResponse = new CompanyUserResponse(userCompany);

        var position = await _positionRepository.AddAsync(new Position
        {
            CompanyId = companyId,
            CreatedByUserId = userId,
            Title = request.Name + " Head",
            InviteCode = GenerateInviteCode(),
            Permissions = PositionPermissions.All,
            ParentPositionId = userCompany.PositionId,
            DepartmentId = department.Id
        }) ?? throw new CannotCreatePositionException();


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
            ParentPositionId = user.PositionId,
            DepartmentId = position.DepartmentId
        }) ?? throw new CannotCreatePositionException();


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

    public Task<ResponseModel<CompanyResponse>> GetCompanyByIdAsync(int companyId)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModel<List<CompanyResponse>>> GetUserCompaniesAsync(Guid userId)
    {
        var companies = await _companyRepository.GetUserCompaniesAsync(userId);

        var response = companies.Select(c => new CompanyResponse(c)).ToList();

        return ResponseModel<List<CompanyResponse>>.Success(response);
    }

    public Task<ResponseModel> DeleteDepartmentAsync(int departmentId, Guid requestedBy)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> DeletePositionAsync(int positionId, Guid requestedBy)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> InviteUserAsync(int companyId, Guid invitedBy, string email, int? positionId)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> ChangeUserRoleAsync(int companyId, Guid userId, string newRole, Guid changedBy)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel> ArchiveCompanyAsync(int companyId, Guid requestedBy)
    {
        throw new NotImplementedException();
    }
    public async Task<ResponseModel<CompanyResponse>> JoinCompanyByInviteAsync(Guid userId, JoinCompanyByInviteRequest request)
    {
        var position = await _positionRepository.GetByInviteCodeAsync(request.InviteCode);

        if (position == null)
            return ResponseModel<CompanyResponse>.Fail("Invalid invite code");

        var existingUser = await _companyUserRepository.GetByUserAndCompany(userId, position.CompanyId);

        if (existingUser != null)
            return ResponseModel<CompanyResponse>.Fail("User already in company");

        var companyUser = new CompanyUser
        {
            UserId = userId,
            CompanyId = position.CompanyId,
            PositionId = position.Id,
            JoinedAt = DateTime.UtcNow,
            IsActive = true
        };

        var departmentChats = await _chatRepository.GetByDepartment(position.DepartmentId);

        foreach (var departmentChat in departmentChats)
        {
            await _chatMemberRepository.AddAsync(new ChatMember
            {
                ChatId = departmentChat.Id,
                UserId = userId
            });
        }

        await _companyUserRepository.AddAsync(companyUser);

        var company = await _companyRepository.GetByIdAsync(position.CompanyId)
            ?? throw new Exception("Company not found");

        return ResponseModel<CompanyResponse>.Success(new CompanyResponse(company));
    }

    public async Task<ResponseModel<List<PositionWithInviteResponse>>> GetUserPositionsAsync(Guid userId, int companyId)
    {
        var positions = await _positionRepository.GetUserPositionsAsync(userId, companyId);

        var dtoList = positions.Select(p => new PositionWithInviteResponse(p.Id, p.Title, p.InviteCode))
            .ToList();

        return ResponseModel<List<PositionWithInviteResponse>>.Success(dtoList);
    }
}