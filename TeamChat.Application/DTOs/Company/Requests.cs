using TeamChat.Domain.Enums;

namespace TeamChat.Application.DTOs.Company;

public record CompanyByIdRequest(int CompanyId, Guid? UserId);
public record CreateCompanyRequest(string Name, Guid DirectorId);
public record UpdateCompanyRequest(string Name, int CompanyId, Guid UserId);
public record UserCompaniesRequest(Guid UserId);
public record AddUserToCompanyRequest(int CompanyId, Guid AdminUserId, Guid UserId, int PositionId);
public record DeactivateUserRequest(int CompanyId, Guid AdminUserId, Guid UserId);
public record CreatePositionRequest(int CompanyId, Guid AdminUserId, string Title, PositionPermissions[] Permissions);
public record UpdatePositionRequest(int CompanyId, Guid AdminUserId, int PositionId, string? Title = null, PositionPermissions? Permissions = null);
public record DeletePositionRequest(int CompanyId, Guid AdminUserId, int PositionId);
public record CheckPermissionRequest(Guid UserId, int CompanyId, PositionPermissions RequiredPermission);
