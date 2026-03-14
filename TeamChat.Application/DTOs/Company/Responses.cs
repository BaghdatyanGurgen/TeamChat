using TeamChat.Domain.Entities;
using TeamChat.Domain.Enums;

namespace TeamChat.Application.DTOs.Company;

public record CompanyResponse(int Id, string Name, string? Description, string? LogoUrl)
{
    public CompanyResponse(Domain.Entities.Company company)
        : this(company.Id, company.Name, company.Description, company.LogoUrl) { }
}

public record SetCompanyDetailsResponse(int Id, string Name, string Description, string? LogoUrl);

public record CreateCompanyDepartmentResponse(int Id, string Name, string? Description)
{
    public CreateCompanyDepartmentResponse(Department department)
        : this(department.Id, department.Name, department.Description) { }
}

public record CreateCompanyPositionResponse(int Id, string Title, int CompanyId, int? ParentPositionId, PositionPermissions Permissions, string InviteCode)
{
    public CreateCompanyPositionResponse(Position position)
        : this(position.Id, position.Title, position.CompanyId, position.ParentPositionId, position.Permissions, position.InviteCode) { }
}

public record JoinCompanyByInviteResponse(int CompanyId, string CompanyName)
{
    public JoinCompanyByInviteResponse(Domain.Entities.Company company)
        : this(company.Id, company.Name) { }
}

public record PositionWithInviteResponse(int Id, string Title, string? InviteCode)
{
    public PositionWithInviteResponse(Position position)
        : this(position.Id, position.Title, position.InviteCode) { }
}