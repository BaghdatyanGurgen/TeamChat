using TeamChat.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace TeamChat.Application.DTOs.Company;

public record CreateCompanyRequest(string Name);
public record SetCompanyDetailsRequest(string Description, IFormFile LogoFile);
public record CreateCompanyDepartmentRequest(string Name, string Description);
public record CreateCompanyPositionRequest(string Title, PositionPermissions Permissions);