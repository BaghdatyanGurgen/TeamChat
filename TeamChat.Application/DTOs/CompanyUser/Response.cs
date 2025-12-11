namespace TeamChat.Application.DTOs.CompanyUser;

public record CompanyUserResponse(int Id, Guid UserId, int CompanyId, int PositionId, DateTime JoinedAt, bool IsActive)
{
    public CompanyUserResponse(Domain.Entities.CompanyUser companyUser) : 
        this(companyUser.Id, companyUser.UserId, companyUser.CompanyId, companyUser.PositionId, companyUser.JoinedAt, companyUser.IsActive)
    {

    }
}