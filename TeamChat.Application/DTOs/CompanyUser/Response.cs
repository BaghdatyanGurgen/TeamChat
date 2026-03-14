namespace TeamChat.Application.DTOs.CompanyUser;

using TeamChat.Domain.Enums;

public record CompanyUserResponse(int Id, Guid UserId, int CompanyId, int PositionId, DateTime JoinedAt, bool IsActive, PositionPermissions Permissions)
{
    public CompanyUserResponse(Domain.Entities.CompanyUser companyUser) : this(companyUser.Id, companyUser.UserId, companyUser.CompanyId, companyUser.PositionId, companyUser.JoinedAt, companyUser.IsActive, companyUser.Position?.Permissions ?? PositionPermissions.None)
    {
    }
}